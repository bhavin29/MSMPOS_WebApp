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
                var query = "SELECT W.Id, W.OutletId,O.OutletName, W.ReferenceNumber, W.WasteDateTime, W.EmployeeId, " +
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
                             " WHERE W.IsDeleted = 0 AND W.Id = "  + purhcaseId;

                purchaseModelList = con.Query<WasteModel>(query).AsList();
            }
            return purchaseModelList;
        }
        public int InsertWaste(WasteModel wasteModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO Waste (OutletId, ReferenceNumber, WasteDateTime, EmployeeId, TotalLossAmount,  ReasonForWaste, WasteStatus,UserIdInserted ,[DateInserted], [IsDeleted])   " +
                             "   VALUES           " +
                             "  (@OutletId, @ReferenceNumber, @WasteDateTime, @EmployeeId, @TotalLossAmount,  @ReasonForWaste, @WasteStatus, 1,   GetUtcDate(),    0); " +
                             "  SELECT CAST(SCOPE_IDENTITY() as int); ";
               
                result = con.ExecuteScalar<int>(query, wasteModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    int detailResult = 0;
                    foreach (var item in wasteModel.WasteDetail)
                    {
                        var queryDetails = "INSERT INTO WasteIngredient ([WasteId],FoodMenuId,[IngredientId] ,IngredientQty, LossAmount, [UserIdUpdated],[IsDeleted])   " +
                                              "VALUES " +
                                              "(" + result + "," + item.FoodMenuId +"," + item.IngredientId +  "," + item.Qty + "," + item.LossAmount +
                                              ",1,0);" +
                                              " SELECT CAST(SCOPE_IDENTITY() as INT); ";
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

            return result;

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
                             "  ,UserIdUpdated = 1, DateUpdated  = GetUtcDate()  where id= " + wasteModel.Id + ";";
  
                result = con.Execute(query, wasteModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    int detailResult = 0;
                    foreach (var item in wasteModel.WasteDetail)
                    {
                        var queryDetails = string.Empty;
                        if (item.WasteId > 0)
                        {
                            queryDetails = "Update [dbo].[WasteIngredient] set " +
                                                 " [IngredientId]  = " + item.IngredientId + "," +
                                                 " [FoodMenuId]   = " + item.FoodMenuId + "," +
                                                 " [IngredientQty]        =  " + item.Qty + "," +
                                                 " [LossAmount] = " + item.LossAmount + 
                                                 " where id = " + item.WasteId + ";";
                        }
                        else
                        {
                            queryDetails = "INSERT INTO [dbo].[WasteIngredient]" +
                                                  " ([WasteId]   " +
                                                  " ,[IngredientId] " +
                                                  " ,[FoodMenuId]    " +
                                                  " ,[IngredientQty]          " +
                                                  " ,[LossAmount]  " +
                                                  " ,[UserIdUpdated]" +
                                                  " ,[IsDeleted])   " +
                                                  "VALUES           " +
                                                  "(" + wasteModel.Id + "," +
                                                  "" + item.IngredientId + "," +
                                                  "" + item.FoodMenuId + "," +
                                                  "" + item.Qty + "," +
                                                  "" + item.LossAmount +
                                                  ",1,0); ";
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
                var query = "select WI.Id as WasteId, WI.FoodMenuId, F.FoodMenuName, WI.IngredientId as IngredientId,i.IngredientName, WI.IngredientQty as Qty ," +
                    " WI.lossAmount as LossAmount from Waste as W " +
                    " inner join WasteIngredient as WI on W.id = WI.WasteId " +
                    " inner join Ingredient i on WI.IngredientId = i.Id " +
                    " inner join FoodMenu F ON F.Id = WI.FoodMenuId " +
                    " where WI.IsDeleted = 0 AND " +
                     " W.id = " + purchaseId + ";";
                purchaseDetails = con.Query<WasteDetailModel>(query).AsList();
            }

            return purchaseDetails;
        }

        public int DeleteWasteDetails(long WasteDetailsId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update WasteIngredient set IsDeleted = 1 where id = " + WasteDetailsId + ";";
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
                var query = $"SELECT ISNULL(MAX(ReferenceNumber),0) + 1 FROM Waste ;";
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
    }
}
