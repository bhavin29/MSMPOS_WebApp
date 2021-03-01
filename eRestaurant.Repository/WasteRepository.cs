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

        public List<WasteListModel> GetWasteList(int foodMenuId, int ingredientId)
        {
            List<WasteListModel> wasteViewModelList = new List<WasteListModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT distinct W.Id, W.StoreId,S.StoreName, W.ReferenceNumber, convert(varchar(12),W.WasteDateTime,3) as WasteDateTime," +
                            " W.TotalLossAmount,  W.ReasonForWaste, W.WasteStatus, isnull(E.Firstname,'') + ' '+  isnull(E.lastname,'') as EmployeeName  " +
                            " FROM Waste W" +
                            " Inner Join WasteIngredient WI On W.Id=WI.WasteId " +
                            " INNER JOIN Store S ON W.StoreId = S.Id " +
                            " inner join [User] U on U.Id=W.UserIdInserted  "+
                            " inner join employee e on e.id = u.employeeid  "+
                            " WHERE W.IsDeleted = 0 ";

                if (foodMenuId != 0)
                {
                    query += " And WI.FoodMenuId= "+ foodMenuId;
                }

                if (ingredientId != 0)
                {
                    query += "  And WI.IngredientId= "+ ingredientId;
                }
                wasteViewModelList = con.Query<WasteListModel>(query).AsList();
            }
            return wasteViewModelList;
        }

        public List<WasteModel> GetWasteById(long purhcaseId)
        {
            List<WasteModel> purchaseModelList = new List<WasteModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT W.Id, W.StoreId,S.StoreName, W.ReferenceNumber, W.WasteDateTime,W.TotalLossAmount,  W.ReasonForWaste, W.WasteStatus " +
                            ",W.CreatedDatetime,W.ApprovedDateTime,UCreatedUser.Username As CreatedUserName,UApprovedUse.Username As ApprovedUserName " +
                            " FROM Waste W " +
                            " INNER JOIN Store S ON W.StoreId = S.Id " +
                            " left join [User] UCreatedUser On UCreatedUser.Id = W.CreatedUserId " +
                            " left join[User] UApprovedUse On UApprovedUse.Id = W.ApprovedUserId " +
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
                                "(StoreId, " +
                                "ReferenceNumber, " +
                                "WasteDateTime, " +
                                "EmployeeId, " +
                                "TotalLossAmount,  " +
                                "ReasonForWaste, " +
                                "WasteStatus," +
                                "UserIdInserted ," +
                                "[DateInserted], " +
                                 "CreatedUserId ," +
                                "[CreatedDatetime], " +
                                "[IsDeleted])   " +
                                "   VALUES           " +
                                "  (@StoreId, " +
                                "@ReferenceNumber, " +
                                "@WasteDateTime, " +
                                "@EmployeeId, " +
                                "@TotalLossAmount,  " +
                                "@ReasonForWaste, " +
                                "@WasteStatus, " +
                                "" + LoginInfo.Userid + "," +
                                "GetUtcDate(),    " +
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
                            queryDetails = "INSERT INTO WasteIngredient ([WasteId],FoodMenuId,[IngredientId] ,IngredientQty, LossAmount, [UserIdInserted],DateInserted,[IsDeleted])   " +
                                                  "VALUES " +
                                                  "(" + result + ",NULL," + item.IngredientId + "," + item.Qty + "," + item.LossAmount +
                                                  "," + LoginInfo.Userid + "," + "GetUtcDate(),0);" +
                                                  " SELECT CAST(ReferenceNumber as INT) from waste where id = " + result + "; ";
                        }
                        else
                        {
                            queryDetails = "INSERT INTO WasteIngredient ([WasteId],FoodMenuId,[IngredientId] ,IngredientQty, LossAmount, [UserIdInserted],DateInserted,[IsDeleted])   " +
                                                 "VALUES " +
                                                 "(" + result + "," + item.FoodMenuId + ",NULL," + item.Qty + "," + item.LossAmount +
                                                 "," + LoginInfo.Userid + "," + "GetUtcDate(),0);" +
                                                 " SELECT CAST(ReferenceNumber as INT) from waste where id = " + result + "; ";
                        }
                        detailResult = con.ExecuteScalar<int>(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                    }

                    if (detailResult > 0)
                    {
                        sqltrans.Commit();

                        if ((int)wasteModel.WasteStatus == 2)
                        {
                            CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                            string sResult = commonRepository.InventoryPush("Waste", result);
                        }
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
                             "StoreId=@StoreId, ReferenceNumber=@ReferenceNumber, WasteDateTime=@WasteDateTime, EmployeeId=@EmployeeId, " +
                             " TotalLossAmount=@TotalLossAmount, ReasonForWaste=@ReasonForWaste, WasteStatus=@WasteStatus ";
                if ((int)wasteModel.WasteStatus == 2)
                {
                    query += ",ApprovedUserId =  " + LoginInfo.Userid + ",ApprovedDateTime=GetUtcDate()";
                }
                query += " ,UserIdUpdated = " + LoginInfo.Userid + ",DateUpdated  = GetUtcDate() where id= " + wasteModel.Id + ";";

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
                        if (item.WasteIngredientId > 0)
                        {
                            if (item.FoodMenuId > 0)
                            {
                                queryDetails = "Update [dbo].[WasteIngredient] set " +
                                               " [IngredientQty] =  " + item.Qty + "," +
                                               " [LossAmount] = " + item.LossAmount + "," +
                                               " UserIdUpdated = " + LoginInfo.Userid + "," +
                                               " DateUpdated = GetUtcDate()" +
                                               " where Id = " + item.WasteIngredientId + ";";
                            }
                            else if (item.IngredientId > 0)
                            {
                                queryDetails = "Update [dbo].[WasteIngredient] set " +
                                                   " [IngredientQty] =  " + item.Qty + "," +
                                                   " [LossAmount] = " + item.LossAmount + "," +
                                                   " UserIdUpdated = " + LoginInfo.Userid + "," +
                                                   " DateUpdated = GetUtcDate()" +
                                                   " where Id = " + item.WasteIngredientId + ";";
                            }
                        }
                        else
                        {
                            if (item.FoodMenuId == 0)
                            {
                                queryDetails = "INSERT INTO WasteIngredient ([WasteId],FoodMenuId,[IngredientId] ,IngredientQty, LossAmount, [UserIdInserted],DateInserted,[IsDeleted])   " +
                                                      "VALUES " +
                                                      "(" + wasteModel.Id + ",NULL," + item.IngredientId + "," + item.Qty + "," + item.LossAmount +
                                                      "," + LoginInfo.Userid + "," + "GetUtcDate(),0);" +
                                                      " SELECT CAST(ReferenceNumber as INT) from waste where id = " + result + "; ";
                            }
                            else
                            {
                                queryDetails = "INSERT INTO WasteIngredient ([WasteId],FoodMenuId,[IngredientId] ,IngredientQty, LossAmount, [UserIdInserted],DateInserted,[IsDeleted])   " +
                                                     "VALUES " +
                                                     "(" + wasteModel.Id + "," + item.FoodMenuId + ",NULL," + item.Qty + "," + item.LossAmount +
                                                     "," + LoginInfo.Userid + "," + "GetUtcDate(),0);" +
                                                     " SELECT CAST(ReferenceNumber as INT) from waste where id = " + result + "; ";
                            }
                        }
                        detailResult = con.Execute(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                    }

                    if (detailResult > 0)
                    {
                        sqltrans.Commit();
                        if ((int)wasteModel.WasteStatus == 2)
                        {
                            CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                            string sResult = commonRepository.InventoryPush("Waste", wasteModel.Id);
                        }

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

        public List<WasteDetailModel> GetWasteDetails(long wasteId)
        {
            List<WasteDetailModel> purchaseDetails = new List<WasteDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {

                var query = " Select WI.Id As WasteIngredientId,WI.WasteId,WI.FoodMenuId,F.FoodMenuName,WI.IngredientId,I.IngredientName,Convert(Numeric(18,2),WI.ingredientqty) As Qty,WI.LossAmount,UF.UnitName As FoodMenuUnitName,UI.UnitName as IngredientUnitName  from WasteIngredient WI " +
                            " left join FoodMenu F ON F.Id=WI.FoodMenuId left join Ingredient I On I.Id=WI.IngredientId left join Units UF On UF.Id=F.UnitsId Left Join Units UI On UI.Id=I.IngredientUnitId " +
                            " where WI.IsDeleted = 0 AND WI.WasteId= " + wasteId + ";";
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

        public decimal GetFoodMenuPurchasePrice(int id)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " Select PurchasePrice From FoodMenu Where IsDeleted=0 And Id=" + id;
                return con.ExecuteScalar<decimal>(query);
            }
        }

        public decimal GetIngredientPurchasePrice(int id)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " Select PurchasePrice From Ingredient Where IsDeleted=0 And Id=" + id;
                return con.ExecuteScalar<decimal>(query);
            }
        }

        public List<WasteDetailModel> GetViewWasteDetails(long wasteId)
        {
            List<WasteDetailModel> wasteDetailModel = new List<WasteDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " Select (case when WI.FoodMenuId is null then 1 else 0 end) as ItemType, (case when WI.FoodMenuId is null then WI.IngredientId else WI.FoodMenuId end) as FoodMenuId,  " +
                            " (case when WI.FoodMenuId is null then I.Ingredientname else f.FoodMenuName end) as FoodMenuName,  WI.Id As WasteIngredientId,WI.WasteId, " +
                            " Convert(Numeric(18,2),WI.ingredientqty) As Qty,(case when WI.FoodMenuId is null then UI.UnitName else UF.UnitName end) as UnitName " +
                            "  from WasteIngredient WI  left join FoodMenu F ON F.Id=WI.FoodMenuId left join Ingredient I On I.Id=WI.IngredientId " +
                            " left join Units UF On UF.Id=F.UnitsId Left Join Units UI On UI.Id=I.IngredientUnitId  " +
                            "  where WI.IsDeleted = 0 AND WI.WasteId=  " + wasteId;
                wasteDetailModel = con.Query<WasteDetailModel>(query).AsList();
            }
            return wasteDetailModel;
        }

        public List<WasteModel> GetViewWasteById(long wasteId)
        {
            List<WasteModel> purchaseModelList = new List<WasteModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT W.Id, W.StoreId,S.StoreName, W.ReferenceNumber, W.WasteDateTime,W.TotalLossAmount,  W.ReasonForWaste, W.WasteStatus " +
                            ",W.CreatedDatetime,W.ApprovedDateTime,UCreatedUser.Username As CreatedUserName,UApprovedUse.Username As ApprovedUserName " +
                            " FROM Waste W " +
                            " INNER JOIN Store S ON W.StoreId = S.Id " +
                            " left join [User] UCreatedUser On UCreatedUser.Id = W.CreatedUserId " +
                            " left join[User] UApprovedUse On UApprovedUse.Id = W.ApprovedUserId " +
                            " WHERE W.IsDeleted = 0 AND W.Id = " + wasteId;

                purchaseModelList = con.Query<WasteModel>(query).AsList();
            }
            return purchaseModelList;
        }
    }
}
