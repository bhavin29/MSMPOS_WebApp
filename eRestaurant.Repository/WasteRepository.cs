using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;
using RocketPOS.Interface.Repository;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using RocketPOS.Framework;

namespace RocketPOS.Repository
{
    public class WasteRepository : IWasteRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;
        public WasteRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<WasteListModel> GetWasteList()
        {
            List<WasteListModel> wasteViewModelList = new List<WasteListModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT W.Id, W.OutletId,O.OutletName, W.ReferenceNumber, convert(varchar(12),W.WasteDateTime,3) as WasteDateTime, W.EmployeeId, " +
                            "(E.LastName + ' ' + E.FirstName) as EmployeeName,W.TotalLossAmount,  W.ReasonForWaste, W.WasteStatus " +
                            " FROM Waste W" +
                            " INNER JOIN Outlet O ON W.OutletId = O.Id " +
                            " INNER JOIN Employee E ON E.Id = W.EmployeeId " +
                            " WHERE W.IsDeleted = 0";
                wasteViewModelList = con.Query<WasteListModel>(query).AsList();
            }


            return wasteViewModelList;
        }

        public List<WasteModel> GetWasteById(long purhcaseId)
        {
            List<WasteModel> purchaseModelList = new List<WasteModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT W.Id, W.OutletId,O.OutletName, W.ReferenceNumber, W.WasteDateTime, W.EmployeeId, (E.LastName + ' ' + E.FirstName) as EmployeeName,W.TotalLossAmount,  W.ReasonForWaste, W.WasteStatus " +
                            " FROM Waste W " +
                            " INNER JOIN Outlet O ON W.OutletId = O.Id " +
                            " INNER JOIN Employee E ON E.Id = W.EmployeeId " +
                             " WHERE W.IsDeleted = 0 AND W.Id = " + purhcaseId;

                purchaseModelList = con.Query<WasteModel>(query).AsList();
            }
            return purchaseModelList;
        }
        public int InsertWaste(WasteModel wasteModel)
        {
            int result = 0;
            int detailResult = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO Waste " +
                                "(OutletId, " +
                                "ReferenceNumber, " +
                                "WasteDateTime, " +
                                "EmployeeId, " +
                                "TotalLossAmount,  " +
                                "ReasonForWaste, " +
                                "WasteStatus," +
                                "UserIdInserted ," +
                                "[DateInserted], " +
                                "[IsDeleted])   " +
                                "   VALUES           " +
                                "  (@OutletId, " +
                                "@ReferenceNumber, " +
                                "@WasteDateTime, " +
                                "@EmployeeId, " +
                                "@TotalLossAmount,  " +
                                "@ReasonForWaste, " +
                                "@WasteStatus, " +
                                "" + LoginInfo.Userid + "," +
                                "GetUtcDate(),    " +
                                "0); SELECT CAST(SCOPE_IDENTITY() as int); ";

                result = con.ExecuteScalar<int>(query, wasteModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    var queryDetails = string.Empty;
                    foreach (var item in wasteModel.WasteDetail)
                    {
                        if (item.FoodMenuId == 0)
                        {
                            queryDetails = "INSERT INTO WasteIngredient ([WasteId],FoodMenuId,[IngredientId] ,IngredientQty, LossAmount, [UserIdInserted],[IsDeleted])   " +
                                                  "VALUES " +
                                                  "(" + result + "," + item.FoodMenuId + "," + item.IngredientId + "," + item.Qty + "," + item.LossAmount +
                                                  "," + LoginInfo.Userid + "," + "0);" +
                                                  " SELECT CAST(ReferenceNumber as INT) from waste where id = " + result + "; ";
                        }
                        else
                        {
                            queryDetails = "INSERT INTO WasteIngredient ([WasteId],FoodMenuId,[IngredientId] ,IngredientQty, LossAmount, [UserIdInserted],[IsDeleted])   " +
                                                  " select " + result + ",FoodMenuId,IngredientId," + item.Qty + "," + item.Qty + " * i.SalesPrice" +
                                                  "," + LoginInfo.Userid + "," +
                                                  "0 from FoodMenuIngredient fmi inner join Ingredient i on fmi.IngredientId = i.id where FoodMenuId =" + item.FoodMenuId + ";" +
                                                  " SELECT CAST(ReferenceNumber as INT) from waste where id = " + result + "; ";
                        }
                        detailResult = con.ExecuteScalar<int>(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                    }

                    if (detailResult > 0)
                    {
                        sqltrans.Commit();
                    }
                    else
                    {
                        sqltrans.Rollback();
                    }
                }
                else
                {
                    sqltrans.Rollback();
                }
            }

            return detailResult;

        }
        public int UpdateWaste(WasteModel wasteModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "Update Waste set " +
                             "OutletId=@OutletId, ReferenceNumber=@ReferenceNumber, WasteDateTime=@WasteDateTime, EmployeeId=@EmployeeId, " +
                             " TotalLossAmount=@TotalLossAmount, ReasonForWaste=@ReasonForWaste, WasteStatus=@WasteStatus " +
                             " ,UserIdUpdated = " + LoginInfo.Userid + ",DateUpdated  = GetUtcDate() where id= " + wasteModel.Id + ";";

                result = con.Execute(query, wasteModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    int detailResult = 0;
                    if (wasteModel.DeletedId != null)
                    {
                        foreach (var item in wasteModel.DeletedId)
                        {
                            var obj = item.Split('|');
                            var deleteQuery = string.Empty;
                            if (Convert.ToInt32(obj[2]) == 0)
                            {
                                deleteQuery = $"update WasteIngredient set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where wasteid = " + Convert.ToInt32(obj[0]) + " and FoodMenuId = " + Convert.ToInt32(obj[1]) + " ;";

                            }
                            else
                            {
                                deleteQuery = $"update WasteIngredient set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where wasteid = " + Convert.ToInt32(obj[0]) + " and " + " IngredientId=" + Convert.ToInt32(obj[2]) + " and FoodMenuId = 0;";
                            }
                            result = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }
                    foreach (var item in wasteModel.WasteDetail)
                    {
                        var queryDetails = string.Empty;
                        if (item.WasteId > 0)
                        {
                            if (item.FoodMenuId > 0)
                            {
                                queryDetails = " update w set w.IngredientQty = " + item.Qty + ", w.LossAmount = " + item.Qty + " * i.SalesPrice" +
                                    ",UserIdUpdated = " + LoginInfo.Userid + ", DateUpdated  = GetUtcDate() from WasteIngredient w " +
                                    " inner join FoodMenuIngredient fmi on w.FoodMenuId = fmi.FoodMenuId inner join Ingredient i on fmi.IngredientId = i.id" +
                                    " where w.FoodMenuId = " + item.FoodMenuId + " and WasteId = " + item.WasteId + " and w.IngredientId = i.id";
                            }
                            else
                            {
                                queryDetails = "Update [dbo].[WasteIngredient] set " +
                                                  " [IngredientQty] =  " + item.Qty + "," +
                                                  " [LossAmount] = " + item.LossAmount + "," +
                                                  " UserIdUpdated = " + LoginInfo.Userid + "," +
                                                  " DateUpdated = GetUtcDate()" +
                                                  " where FoodMenuId = 0 and WasteId = " + item.WasteId + " and IngredientId = " + item.IngredientId + ";";
                            }
                        }
                        else
                        {
                            if (item.FoodMenuId == 0)
                            {
                                queryDetails = "INSERT INTO WasteIngredient ([WasteId],FoodMenuId,[IngredientId] ,IngredientQty, LossAmount, [UserIdInserted],[IsDeleted])   " +
                                                       "VALUES " +
                                                       "(" + wasteModel.Id + "," + item.FoodMenuId + "," + item.IngredientId + "," + item.Qty + "," + item.LossAmount +
                                                       "," + LoginInfo.Userid + ",0);" +
                                                       " SELECT CAST(ReferenceNumber as INT) from waste where id = " + result + "; ";
                            }
                            else
                            {
                                queryDetails = "INSERT INTO WasteIngredient ([WasteId],FoodMenuId,[IngredientId] ,IngredientQty, LossAmount, [UserIdInserted],[IsDeleted])   " +
                                                       " select " + wasteModel.Id + ",FoodMenuId,IngredientId," + item.Qty + "," + item.LossAmount +
                                                        "," + LoginInfo.Userid + ",0 " +
                                                       " from FoodMenuIngredient where FoodMenuId = " + item.FoodMenuId + ";" +
                                                       " SELECT CAST(ReferenceNumber as INT) from waste where id = " + result + "; ";
                            }


                        }
                        detailResult = con.Execute(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                    }

                    if (detailResult > 0)
                    {
                        sqltrans.Commit();
                    }
                    else
                    {
                        sqltrans.Rollback();
                    }
                }
                else
                {
                    sqltrans.Rollback();
                }
            }

            return result;
        }
        public int DeleteWaste(long WasteId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update Waste set IsDeleted = 1 where id = " + WasteId + ";" +
                    " Update WasteIngredient set IsDeleted = 1 where WasteId = " + WasteId + ";";
                result = con.Execute(query, null, sqltrans, 0, System.Data.CommandType.Text);
                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                { sqltrans.Rollback(); }
            }
            return result;
        }

        public List<WasteDetailModel> GetWasteDetails(long purchaseId)
        {
            List<WasteDetailModel> purchaseDetails = new List<WasteDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {

                var query = "select W.Id as WasteId, WI.FoodMenuId, F.FoodMenuName, '0' as IngredientId,'' IngredientName, WI.IngredientQty as Qty , Sum(WI.lossAmount) as LossAmount " +
                            " from Waste as W  inner join WasteIngredient as WI on W.id = WI.WasteId  inner join Ingredient i on WI.IngredientId = i.Id  inner join FoodMenu F ON F.Id = WI.FoodMenuId" +
                            " where WI.IsDeleted = 0 AND W.id = " + purchaseId + " group by WI.IngredientQty,W.Id,WI.FoodMenuId,F.FoodMenuName" +
                            " union all" +
                            " select W.Id as WasteId, '0', '', i.id as IngredientId,i.IngredientName, WI.IngredientQty as Qty , WI.lossAmount as LossAmount" +
                            " from Waste as W  inner join WasteIngredient as WI on W.id = WI.WasteId  inner join Ingredient i on WI.IngredientId = i.Id  and WI.FoodMenuId = 0" +
                            " where WI.IsDeleted = 0 AND W.id = " + purchaseId + ";";
                //var query = "select WI.Id as WasteId, WI.FoodMenuId, F.FoodMenuName, WI.IngredientId as IngredientId,i.IngredientName, WI.IngredientQty as Qty ," +
                //    " WI.lossAmount as LossAmount from Waste as W " +
                //    " inner join WasteIngredient as WI on W.id = WI.WasteId " +
                //    " inner join Ingredient i on WI.IngredientId = i.Id " +
                //    " inner join FoodMenu F ON F.Id = WI.FoodMenuId " +
                //    " where WI.IsDeleted = 0 AND " +
                //     " W.id = " + purchaseId + ";";
                purchaseDetails = con.Query<WasteDetailModel>(query).AsList();
            }

            return purchaseDetails;
        }

        public int DeleteWasteDetails(long wasteId, long foodManuId, long ingredientId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = string.Empty;
                if (ingredientId == 0)
                {
                    query = $"update WasteIngredient set IsDeleted = 1 where wasteid = " + wasteId + " and FoodMenuId = " + foodManuId + " ;";

                }
                else
                {
                    query = $"update WasteIngredient set IsDeleted = 1 where wasteid = " + wasteId + " and " + " IngredientId=" + ingredientId + " and FoodMenuId = 0;";
                }
                result = con.Execute(query, null, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                { sqltrans.Rollback(); }
            }
            return result;
        }

        public long ReferenceNumber()
        {
            long result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"SELECT ISNULL(max(CONVERT(numeric, ReferenceNumber)),0) + 1 FROM Waste;";
                result = con.ExecuteScalar<long>(query, null, sqltrans, 0, System.Data.CommandType.Text);
                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                { sqltrans.Rollback(); }
            }
            return result;
        }

        public List<DropDownModel> IngredientListForLostAmount()
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Id,SalesPrice as [Name] from Ingredient where IsDeleted= 0 Order by IngredientName";
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }

        public List<DropDownModel> FoodMenuListForLostAmount()
        {
            List<DropDownModel> dropDownModels = new List<DropDownModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select FoodMenuId as Id,sum(i.SalesPrice) as [Name] from FoodMenu as F inner join FoodMenuIngredient as FMI on f.Id=FMI.FoodMenuId" +
                                " inner join Ingredient as i on i.id = fmi.IngredientId" +
                                " where F.IsDeleted = 0 group by FoodMenuId";
                dropDownModels = con.Query<DropDownModel>(query).ToList();
            }
            return dropDownModels;
        }

    }
}
