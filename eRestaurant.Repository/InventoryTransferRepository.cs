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
    public class InventoryTransferRepository :IInventoryTransferRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;
        public InventoryTransferRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }


        public int DeleteInventoryTransfer(long invAdjId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update InventoryTransfer set IsDeleted = 1 where id = " + invAdjId + ";" +
                    " Update [InventoryTransferIngredient] set IsDeleted = 1 where [InventoryTransferId] = " + invAdjId + ";";
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

        public int DeleteInventoryTransferDetail(long invAdjDetailId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update [InventoryTransferIngredient] set IsDeleted = 1 where id = " + invAdjDetailId + ";";
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

        public List<InventoryTransferModel> GetInventoryTransferById(long invAdjId)
        {
            List<InventoryTransferModel> inventoryTransferModels = new List<InventoryTransferModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT IA.Id,IA.FromStoreId,S.StoreName As FromStoreName,IA.ToStoreId,SS.StoreName AS ToStoreName,  IA.ReferenceNumber,IA.EntryDate,IA.EmployeeId,E.LastName + E.FirstName as EmployeeName, " +
                            "IA.Notes FROM[RocketPOS].[dbo].[InventoryTransfer] IA INNER JOIN Employee E ON E.Id = IA.EmployeeId " +
                            "INNER JOIN Store S ON S.Id = IA.ToStoreId " +
                            "INNER JOIN Store SS ON SS.Id = IA.FromStoreId " +
                            "WHERE IA.IsDeleted = 0 AND IA.Id = " + invAdjId;
                inventoryTransferModels = con.Query<InventoryTransferModel>(query).AsList();
            }
            return inventoryTransferModels;
        }

        public List<InventoryTransferDetailModel> GetInventoryTransferDetail(long invAdjId)
        {
            List<InventoryTransferDetailModel> inventoryTransferDetailModels = new List<InventoryTransferDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT IAI.Id,IAI.InventoryTransferId,IAI.IngredientId,I.IngredientName,IAI.IntgredientQty,IAI.ConsumptionStatus " +
                             " FROM InventoryTransferIngredient IAI " +
                             " INNER JOIN InventoryTransferIngredient IA ON IAI.InventoryTransferId = IA.Id " +
                             " INNER JOIN Ingredient I ON I.Id = IAI.IngredientId WHERE IAI.InventoryTransferId= " + invAdjId;
                inventoryTransferDetailModels = con.Query<InventoryTransferDetailModel>(query).AsList();
            }

            return inventoryTransferDetailModels;

        }

        public List<InventoryTransferViewModel> GetInventoryTransferList()
        {
            List<InventoryTransferViewModel> inventoryTransferModels = new List<InventoryTransferViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT IA.Id,IA.FromStoreId,S.StoreName As FromStoreName,IA.ToStoreId,SS.StoreName as ToStoreName,IA.ReferenceNumber,IA.EntryDate,IA.EmployeeId,E.LastName + E.FirstName as EmployeeName,  IA.Notes " +
                              "FROM[RocketPOS].[dbo].[InventoryTransfer] IA INNER JOIN Employee E ON E.Id = IA.EmployeeId " +
                              "INNER JOIN Store S ON S.Id = IA.FromStoreId " +
                              "INNER JOIN Store SS ON SS.Id = IA.ToStoreId " +
                              "WHERE IA.IsDeleted = 0 ;";
                inventoryTransferModels = con.Query<InventoryTransferViewModel>(query).AsList();
            }


            return inventoryTransferModels;
        }

        public int InsertInventoryTransfer(InventoryTransferModel inventoryTransferModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO [InventoryTransfer] " +
                             "  ( Id ,FromStoreId, ToStoreId,ReferenceNumber,EntryDate ,EmployeeId,Notes ) " +
                             "   VALUES           " +
                             "  ( @Id ,@FromStoreId, @ToStoreId,@ReferenceNumber,@EntryDate ,@EmployeeId,@Notes ); " +
                             "   SELECT CAST(Scope_Identity()  as int); ";
                result = con.ExecuteScalar<int>(query, inventoryTransferModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    int detailResult = 0;
                    foreach (var item in inventoryTransferModel.InventoryTransferDetail)
                    {
                        var queryDetails = "INSERT INTO InventoryTransferIngredient" +
                                              " (Id,InventoryTransferId,IngredientId,IntgredientQty,ConsumptionStatus) " +
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
                var query = $"SELECT ISNULL(MAX(ReferenceNumber),0) + 1 FROM InventoryTransfer ;";
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

        public int UpdateInventoryTransfer(InventoryTransferModel inventoryTransferModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "Update  InventoryTransfer SET FromStoreId=@FromStoreId,ToStoreId=@ToStoreId, ReferenceNumber=@ReferenceNumber,EntryDate=@EntryDate ,EmployeeId=@,Notes " +
                             ",[UserIdUpdated] = 1 ,[DateUpdated]  = GetUtcDate()  where id= " + inventoryTransferModel.Id + ";";
                result = con.Execute(query, inventoryTransferModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    int detailResult = 0;
                    foreach (var item in inventoryTransferModel.InventoryTransferDetail)
                    {
                        var queryDetails = string.Empty;
                        if (item.InventoryTransferId > 0)
                        {

                            queryDetails = "Update InventoryTransferIngredient SET InventoryTransferId, IngredientId,IntgredientQty,ConsumptionStatus where id = " + item.InventoryTransferId + ";";


                            queryDetails = "Update InventoryTransferIngredient SET " +
                                                 " ,IngredientId = " + item.IngredientId +
                                                 " ,IntgredientQty   = " + item.Quantity +
                                                 ",ConsumptionStatus=  " + item.ConsumpationStatus +
                                                 " WHERE Id = " + item.InventoryTransferId + ";";
                        }
                        else
                        {
                            queryDetails = "INSERT INTO InventoryTransferIngredient" +
                                                  " (InventoryTransferId, IngredientId,IntgredientQty,ConsumptionStatus,UserIdUpdated,IsDeleted)   " +
                                                  "VALUES           " +
                                                  "(" + inventoryTransferModel.Id + "," +
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
