using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;
using Dapper;
using RocketPOS.Interface.Repository;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

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
                    " Update [InventoryAdjustmentIngredient] set IsDeleted = 1 where [InventoryAdjustmentId] = " + invAdjId + ";";
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
                var query = $"update [InventoryAdjustmentIngredient] set IsDeleted = 1 where id = " + invAdjDetailId + ";";
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
                var query = "SELECT IA.Id,IA.StoreId,S.StoreName,IA.ReferenceNumber,IA.EntryDate,IA.EmployeeId,E.LastName + E.FirstName as EmployeeName,  IA.Notes " +
                              "FROM[RocketPOS].[dbo].[InventoryAdjustment] IA INNER JOIN Employee E ON E.Id = IA.EmployeeId " +
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
                var query = "SELECT IAI.Id,IAI.InventoryAdjustmentId,IAI.IngredientId,I.IngredientName,IAI.IntgredientQty,IAI.ConsumptionStatus " +
                             " FROM InventoryAdjustmentIngredient IAI " +
                             " INNER JOIN InventoryAdjustmentIngredient IA ON IAI.InventoryAdjustmentId = IA.Id " +
                             " INNER JOIN Ingredient I ON I.Id = IAI.IngredientId " + invAdjId;
                 inventoryAdjustmentDetailModels = con.Query<InventoryAdjustmentDetailModel>(query).AsList();
            }

            return inventoryAdjustmentDetailModels;

        }

        public List<InventoryAdjustmentViewModel> GetInventoryAdjustmentList()
        {
            List<InventoryAdjustmentViewModel> inventoryAdjustmentModels = new List<InventoryAdjustmentViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT IA.Id,IA.StoreId,S.StoreName,IA.ReferenceNumber,IA.EntryDate,IA.EmployeeId,E.LastName + E.FirstName as EmployeeName,  IA.Notes " +
                              "FROM[RocketPOS].[dbo].[InventoryAdjustment] IA INNER JOIN Employee E ON E.Id = IA.EmployeeId " +
                              "INNER JOIN Store S ON S.Id = IA.StoreId " +
                              "WHERE IA.IsDeleted = 0 ;";
                inventoryAdjustmentModels = con.Query<InventoryAdjustmentViewModel>(query).AsList();
            }


            return inventoryAdjustmentModels;
        }

        public int InsertInventoryAdjustment(InventoryAdjustmentModel inventoryAdjustmentModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO [InventoryAdjustment] " +
                             "  ( Id ,StoreId, ReferenceNumber,EntryDate ,EmployeeId,Notes ) " + 
                             "   VALUES           " +
                             "  ( @Id ,@StoreId, @ReferenceNumber,@EntryDate ,@EmployeeId,@Notes ); " +
                             "   SELECT CAST(Scope_Identity()  as int); ";
                result = con.ExecuteScalar<int>(query, inventoryAdjustmentModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    int detailResult = 0;
                    foreach (var item in inventoryAdjustmentModel.InventoryAdjustmentDetail)
                    {
                        var queryDetails = "INSERT INTO InventoryAdjustmentIngredient" +
                                              " (Id,InventoryAdjustmentId,IngredientId,IntgredientQty,ConsumptionStatus) " +
                                              "VALUES           " +
                                              "(" + result + ",@IngredientId,@IntgredientQty,@ConsumptionStatus); " +
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
                var query = "Update  InventoryAdjustment SET StoreId=@StoreId, ReferenceNumber=@ReferenceNumber,EntryDate=@EntryDate ,EmployeeId=@,Notes " +
                             ",[UserIdUpdated] = 1 ,[DateUpdated]  = GetUtcDate()  where id= " + inventoryAdjustmentModel.Id + ";";
                result = con.Execute(query, inventoryAdjustmentModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    int detailResult = 0;
                    foreach (var item in inventoryAdjustmentModel.InventoryAdjustmentDetail)
                    {
                        var queryDetails = string.Empty;
                        if (item.InventoryAdjustmentId > 0)
                        {

                            queryDetails = "Update InventoryAdjustmentIngredient SET InventoryAdjustmentId, IngredientId,IntgredientQty,ConsumptionStatus where id = " + item.InventoryAdjustmentId + ";";


                            queryDetails = "Update InventoryAdjustmentIngredient SET " +
                                                 " ,IngredientId = " + item.IngredientId +
                                                 " ,IntgredientQty   = " + item.Quantity +
                                                 ",ConsumptionStatus=  " + item.ConsumpationStatus +
                                                 " WHERE Id = " + item.InventoryAdjustmentId + ";";
                        }
                        else
                        {
                            queryDetails = "INSERT INTO [dbo].[PurchaseIngredient]" +
                                                  " (InventoryAdjustmentId, IngredientId,IntgredientQty,ConsumptionStatus,UserIdUpdated,IsDeleted)   " +
                                                  "VALUES           " +
                                                  "(" + inventoryAdjustmentModel.Id + "," +
                                                  "" + item.IngredientId + "," +
                                                  "" + item.Quantity + "," +
                                                  "" + item.ConsumpationStatus + ",1,0)";
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
