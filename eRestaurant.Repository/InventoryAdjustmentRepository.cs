using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;
using Dapper;
using RocketPOS.Interface.Repository;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using RocketPOS.Framework;

namespace RocketPOS.Repository
{
    public class InventoryAdjustmentRepository : IInventoryAdjustmentRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;
        public InventoryAdjustmentRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }


        public int DeleteInventoryAdjustment(long invAdjId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update InventoryAdjustment set IsDeleted = 1 where id = " + invAdjId + ";" +
                    " Update InventoryAdjustmentIngredient set IsDeleted = 1 where [InventoryAdjustmentId] = " + invAdjId + ";";
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

        public int DeleteInventoryAdjustmentDetail(long invAdjDetailId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update InventoryAdjustmentIngredient set IsDeleted = 1 where id = " + invAdjDetailId + ";";
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

        public List<InventoryAdjustmentModel> GetInventoryAdjustmentById(long invAdjId)
        {
            List<InventoryAdjustmentModel> inventoryAdjustmentModels = new List<InventoryAdjustmentModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT IA.Id,IA.StoreId,S.StoreName,IA.ReferenceNumber as ReferenceNo,IA.EntryDate as Date,IA.EmployeeId,E.LastName + E.FirstName as EmployeeName,  IA.Notes " +
                              "FROM InventoryAdjustment IA INNER JOIN Employee E ON E.Id = IA.EmployeeId " +
                              "INNER JOIN Store S ON S.Id = IA.StoreId " +
                              "WHERE IA.IsDeleted = 0 AND IA.Id = " + invAdjId;
                inventoryAdjustmentModels = con.Query<InventoryAdjustmentModel>(query).AsList();
            }
            return inventoryAdjustmentModels;
        }

        public List<InventoryAdjustmentDetailModel> GetInventoryAdjustmentDetail(long invAdjId)
        {
            List<InventoryAdjustmentDetailModel> inventoryAdjustmentDetailModels = new List<InventoryAdjustmentDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT IAI.Id as InventoryAdjustmentId,IAI.IngredientId,I.IngredientName,IAI.IntgredientQty as Quantity, IAI.ConsumptionStatus as ConsumpationStatus " +
                             " FROM InventoryAdjustmentIngredient IAI " +
                             " INNER JOIN InventoryAdjustment IA ON IAI.InventoryAdjustmentId = IA.Id " +
                             " INNER JOIN Ingredient I ON I.Id = IAI.IngredientId " +
                             " where IA.Id =" + invAdjId + " and IA.IsDeleted = 0 and IAI.IsDeleted = 0;";
                inventoryAdjustmentDetailModels = con.Query<InventoryAdjustmentDetailModel>(query).AsList();
            }

            return inventoryAdjustmentDetailModels;

        }

        public List<InventoryAdjustmentViewModel> GetInventoryAdjustmentList()
        {
            List<InventoryAdjustmentViewModel> inventoryAdjustmentModels = new List<InventoryAdjustmentViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT IA.Id,IA.StoreId,S.StoreName,IA.ReferenceNumber as ReferenceNo,convert(varchar(12),IA.EntryDate, 3) as [Date],IA.EmployeeId,E.LastName + E.FirstName as EmployeeName,  IA.Notes " +
                              "FROM InventoryAdjustment IA INNER JOIN Employee E ON E.Id = IA.EmployeeId " +
                              "INNER JOIN Store S ON S.Id = IA.StoreId " +
                              "WHERE IA.IsDeleted = 0 ;";
                inventoryAdjustmentModels = con.Query<InventoryAdjustmentViewModel>(query).AsList();
            }


            return inventoryAdjustmentModels;
        }

        public int InsertInventoryAdjustment(InventoryAdjustmentModel inventoryAdjustmentModel)
        {
            int result = 0;
            int detailResult = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO InventoryAdjustment " +
                             "  ( StoreId, ReferenceNumber,EntryDate ,EmployeeId,Notes,UserIdUpdated,DateInserted,IsDeleted ) " +
                             "   VALUES           " +
                             "  ( @StoreId, @ReferenceNo, @Date ,@EmployeeId,@Notes,"+ LoginInfo.Userid +",GetUTCDate(),0); " +
                             "   SELECT CAST(Scope_Identity()  as int); ";
                result = con.ExecuteScalar<int>(query, inventoryAdjustmentModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {

                    foreach (var item in inventoryAdjustmentModel.InventoryAdjustmentDetail)
                    {
                        int consumptionId = 0;
                        if (item.ConsumpationStatus.Value.ToString() == "StockIN")
                        {
                            consumptionId = 1;
                        }
                        else
                        {
                            consumptionId = 2;
                        }
                        var queryDetails = "INSERT INTO InventoryAdjustmentIngredient" +
                                              " (InventoryAdjustmentId,IngredientId,IntgredientQty,ConsumptionStatus,UserIdUpdated,DateInserted,IsDeleted) " +
                                              "VALUES           " +
                                              "(" + result + "," +
                                              "" + item.IngredientId + "," +
                                              "" + item.Quantity + "," +
                                              "" + consumptionId + "," +
                                              "" + LoginInfo.Userid +",GetUtcDate(),0); " +
                                              " SELECT CAST(ReferenceNumber as INT) from InventoryAdjustment where id = " + result + "; ";
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

        public long ReferenceNumber()
        {
            long result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"SELECT ISNULL(MAX(ReferenceNumber),0) + 1 FROM InventoryAdjustment ;";
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

        public int UpdateInventoryAdjustment(InventoryAdjustmentModel inventoryAdjustmentModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();

                var query = "Update  InventoryAdjustment SET StoreId=@StoreId, ReferenceNumber=@ReferenceNo,EntryDate=@Date ,EmployeeId=@EmployeeId,Notes = @Notes" +
                             ",[UserIdUpdated] =  " + LoginInfo.Userid + ", [DateUpdated]  = GetUtcDate()  where id= " + inventoryAdjustmentModel.Id + ";";
                result = con.Execute(query, inventoryAdjustmentModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    int detailResult = 0;
                    if (inventoryAdjustmentModel.DeletedId != null)
                    {
                        foreach (var item in inventoryAdjustmentModel.DeletedId)
                        {
                            var deleteQuery = $"update InventoryAdjustmentIngredient set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                            result = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }

                    foreach (var item in inventoryAdjustmentModel.InventoryAdjustmentDetail)
                    {
                        var queryDetails = string.Empty;
                        int consumptionId = 0;
                        if (item.ConsumpationStatus.Value.ToString() == "StockIN")
                        {
                            consumptionId = 1;
                        }
                        else
                        {
                            consumptionId = 2;
                        }
                        if (item.InventoryAdjustmentId > 0)
                        {

                            queryDetails = "Update InventoryAdjustmentIngredient SET " +
                                                 " IngredientId = " + item.IngredientId +
                                                 " ,IntgredientQty   = " + item.Quantity +
                                                 ",ConsumptionStatus=  " + consumptionId +
                                                  ",UserIdUpdated = " + LoginInfo.Userid +
                                                  ", DateUpdated  = GetUtcDate()"+
                                                 " WHERE Id = " + item.InventoryAdjustmentId + ";";
                        }
                        else
                        {
                            queryDetails = "INSERT INTO InventoryAdjustmentIngredient" +
                                                  " (InventoryAdjustmentId, IngredientId,IntgredientQty,ConsumptionStatus,UserIdUpdated,DateUpdated,IsDeleted)   " +
                                                  "VALUES           " +
                                                  "(" + inventoryAdjustmentModel.Id + "," +
                                                  "" + item.IngredientId + "," +
                                                  "" + item.Quantity + "," +
                                                  "" + consumptionId + "," + LoginInfo.Userid + ",GetUTCDate(),0)";
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
    }

}
