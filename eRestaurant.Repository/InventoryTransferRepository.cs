﻿using System;
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
    public class InventoryTransferRepository : IInventoryTransferRepository
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
                var query = $"update InventoryTransfer set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where id = " + invAdjId + ";" +
                    " Update [InventoryTransferDetail] set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where [InventoryTransferId] = " + invAdjId + ";";
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
                var query = $"update [InventoryTransferDetail] set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where id = " + invAdjDetailId + ";";
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
                var query = "SELECT IA.Id,IA.InventoryType,IA.FromStoreId,S.StoreName As FromStoreName, IA.ToStoreId ,SS.StoreName AS ToStoreName, " +
                    " IA.ReferenceNumber as ReferenceNo, IA.EntryDate as [Date], IA.EmployeeId, E.LastName + E.FirstName as EmployeeName, " +
                            "IA.Notes FROM [InventoryTransfer] IA LEFT JOIN Employee E ON E.Id = IA.EmployeeId " +
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
                var query = "SELECT IAI.Id AS InventoryTransferId,IAI.IngredientId,I.IngredientName,IAI.FoodMenuId, FM.FoodMenuName,IAI.Qty as Quantity,IAI.ConsumptionStatus as ConsumpationStatus, IAI.CurrentStock " +
                             " FROM InventoryTransferDetail IAI " +
                             " INNER JOIN InventoryTransfer IA ON IAI.InventoryTransferId = IA.Id " +
                             " LEFT JOIN Ingredient I ON I.Id = IAI.IngredientId " +
                             " LEFT JOIN FoodMenu FM ON FM.Id = IAI.FoodMenuId " +
                             "WHERE IAI.InventoryTransferId= " + invAdjId + " and IA.IsDeleted = 0 and IAI.IsDeleted = 0;";
                inventoryTransferDetailModels = con.Query<InventoryTransferDetailModel>(query).AsList();
            }

            return inventoryTransferDetailModels;

        }

        public List<InventoryTransferViewModel> GetInventoryTransferList()
        {
            List<InventoryTransferViewModel> inventoryTransferModels = new List<InventoryTransferViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT IA.Id,IA.InventoryType,IA.FromStoreId,S.StoreName As FromStoreName,IA.ToStoreId,SS.StoreName as ToStoreName,IA.ReferenceNumber as ReferenceNo,convert(varchar(12),IA.EntryDate, 3) as [Date],IA.EmployeeId,E.LastName + EE.FirstName as EmployeeName,  IA.Notes ,isnull(EE.Firstname,'') + ' '+  isnull(E.lastname,'') as Username  " +
                              "FROM [InventoryTransfer] IA LEFT JOIN Employee E ON E.Id = IA.EmployeeId " +
                              "INNER JOIN Store S ON S.Id = IA.FromStoreId " +
                              "INNER JOIN Store SS ON SS.Id = IA.ToStoreId " +
                              "inner join [User] U on U.Id=IA.UserIdInserted  inner join employee ee on ee.id = u.employeeid " +
                              "WHERE IA.IsDeleted = 0 Order by IA.Id desc;";
                inventoryTransferModels = con.Query<InventoryTransferViewModel>(query).AsList();
            }


            return inventoryTransferModels;
        }

        public List<InventoryTransferViewModel> GetInventoryTransferListByDate(string fromDate, string toDate)
        {
            List<InventoryTransferViewModel> inventoryTransferModels = new List<InventoryTransferViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT IA.Id,IA.InventoryType,IA.FromStoreId,S.StoreName As FromStoreName,IA.ToStoreId,SS.StoreName as ToStoreName,IA.ReferenceNumber as ReferenceNo,convert(varchar(12),IA.EntryDate, 3) as [Date],IA.EmployeeId,E.LastName + E.FirstName as EmployeeName,  IA.Notes ,isnull(EE.Firstname,'') + ' '+  isnull(EE.lastname,'') as Username  " +
                              "FROM [InventoryTransfer] IA LEFT JOIN Employee E ON E.Id = IA.EmployeeId " +
                              "INNER JOIN Store S ON S.Id = IA.FromStoreId " +
                              "INNER JOIN Store SS ON SS.Id = IA.ToStoreId " +
                               "inner join [User] U on U.Id=IA.UserIdInserted  inner join employee ee on ee.id = u.employeeid " +
                              "WHERE IA.IsDeleted = 0 " +
                            //"And Convert(varchar(10),IA.EntryDate,103)  between '" + fromDate + "' and '" + toDate + "'  Order by IA.EntryDate DESC";
                            " AND Convert(Date, IA.EntryDate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103)  ";

                query += " order by IA.EntryDate,IA.DateInserted;";


                inventoryTransferModels = con.Query<InventoryTransferViewModel>(query).AsList();
            }


            return inventoryTransferModels;
        }

        public int InsertInventoryTransfer(InventoryTransferModel inventoryTransferModel)
        {
            int result = 0;
            int detailResult = 0;
            string foodMenuId = "NULL", ingredientId = "NULL", assetItemId = "NULL";
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO [InventoryTransfer] " +
                             "  ( FromStoreId, ToStoreId,ReferenceNumber,InventoryType,EntryDate ,EmployeeId,Notes,UserIdInserted,DateInserted,IsDeleted  ) " +
                             "   VALUES           " +
                             "  ( @FromStoreId, @ToStoreId,@ReferenceNo,@InventoryType,@Date ,@EmployeeId,@Notes," + LoginInfo.Userid + ",GetUTCDate(),0); " +
                             "   SELECT CAST(Scope_Identity()  as int); ";
                result = con.ExecuteScalar<int>(query, inventoryTransferModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    foreach (var item in inventoryTransferModel.InventoryTransferDetail)
                    {
                        /*
                        int consumptionId = 0;
                        if (item.ConsumpationStatus.Value.ToString() == "StockIN")
                        {
                            consumptionId = 1;
                        }
                        else
                        {
                            consumptionId = 2;
                        }
                        */
                        if (item.IngredientId == 0)
                        {
                            ingredientId = "NULL";
                        }
                        else
                        {
                            ingredientId = item.IngredientId.ToString();
                        }

                        if (item.AssetItemId == 0)
                        {
                            assetItemId = "NULL";
                        }
                        else
                        {
                            assetItemId = item.AssetItemId.ToString();
                        }

                        if (item.FoodMenuId == 0)
                        {
                            foodMenuId = "NULL";
                        }
                        else
                        {
                            foodMenuId = item.FoodMenuId.ToString();
                        }

                        var queryDetails = "INSERT INTO InventoryTransferDetail" +
                                              " (InventoryTransferId,IngredientId,FoodMenuId,AssetItemId,Qty,ConsumptionStatus,CurrentStock,UserIdInserted,DateInserted,IsDeleted) " +
                                              "VALUES           " +
                                              "(" + result + "," +
                                              "" + ingredientId + "," +
                                              "" + foodMenuId + "," +
                                              "" + assetItemId + "," +
                                              "" + item.Quantity + "," +
                                              "" + 1 + "," +
                                              "" + item.CurrentStock + "," +
                                               "" + LoginInfo.Userid + ",GetUtcDate(),0); " +
                                              " SELECT CAST(ReferenceNumber as INT) from [InventoryTransfer] where id = " + result + "; ";
                        detailResult = con.ExecuteScalar<int>(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);

                    }
                    if (detailResult > 0)
                    {
                        sqltrans.Commit();

                        CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                        string sResult = commonRepository.InventoryPush("IT", result);

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
                var query = $"SELECT ISNULL(MAX(convert(int,ReferenceNumber)),0) + 1 FROM InventoryTransfer where ISDeleted=0 ;";
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
            string foodMenuId = "NULL", ingredientId = "NULL", assetItemId = "NULL";
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "Update  InventoryTransfer SET FromStoreId=@FromStoreId,EntryDate=@Date,ToStoreId=@ToStoreId, ReferenceNumber=@ReferenceNo,EmployeeId=@EmployeeId, Notes=@Notes " +
                             ",[UserIdUpdated] = " + LoginInfo.Userid + " ,[DateUpdated]  = GetUtcDate()  where id= " + inventoryTransferModel.Id + ";";
                result = con.Execute(query, inventoryTransferModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    int detailResult = 0;
                    if (inventoryTransferModel.DeletedId != null)
                    {
                        foreach (var item in inventoryTransferModel.DeletedId)
                        {
                            var deleteQuery = $"update InventoryTransferDetail set IsDeleted = 1,  UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                            result = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }
                    foreach (var item in inventoryTransferModel.InventoryTransferDetail)
                    {
                        var queryDetails = string.Empty;
                        /*
                        int consumptionId = 0;
                        if (item.ConsumpationStatus.Value.ToString() == "StockIN")
                        {
                            consumptionId = 1;
                        }
                        else
                        {
                            consumptionId = 2;
                        }
                        */
                        if (item.IngredientId == 0)
                        {
                            ingredientId = "NULL";
                        }
                        else
                        {
                            ingredientId = item.IngredientId.ToString();
                        }

                        if (item.AssetItemId == 0)
                        {
                            assetItemId = "NULL";
                        }
                        else
                        {
                            assetItemId = item.AssetItemId.ToString();
                        }

                        if (item.FoodMenuId == 0)
                        {
                            foodMenuId = "NULL";
                        }
                        else
                        {
                            foodMenuId = item.FoodMenuId.ToString();
                        }

                        if (item.InventoryTransferId > 0)
                        {
                            queryDetails = "Update InventoryTransferDetail SET " +
                                " IngredientId = " + ingredientId +
                                " ,FoodMenuId = " + foodMenuId +
                                " ,AssetItemId = " + assetItemId +
                                " ,Qty = " + item.Quantity +
                                " ,ConsumptionStatus = " + 1 +
                                " ,CurrentStock = " + item.CurrentStock +
                                " ,UserIdUpdated = " + LoginInfo.Userid +
                                " ,DateUpdated = GetUTCDate()" +
                                " ,IsDeleted = 0" +
                                " where id = " + item.InventoryTransferId + ";";
                        }
                        else
                        {
                            queryDetails = "INSERT INTO InventoryTransferDetail" +
                                                  " (InventoryTransferId, IngredientId,FoodMenuId,AssetItemId,Qty,ConsumptionStatus,CurrentStock,UserIdUpdated,DateInserted,IsDeleted)   " +
                                                  "VALUES           " +
                                                  "(" + inventoryTransferModel.Id + "," +
                                                  "" + ingredientId + "," +
                                                  "" + foodMenuId + "," +
                                                  "" + assetItemId + "," +
                                                  "" + item.Quantity + "," +
                                                  "" + 1 + "," +
                                                  "" + item.CurrentStock + "," +
                                                  "" + LoginInfo.Userid + ",GetUTCDate(),0)";
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

        public decimal GetFoodMenuStock(int foodMenuId, int storeId,int inventoryType)
        {
            decimal result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();

                var query = "";

                if (inventoryType == 1)
                {
                     query = " SELECT StockQty from Inventory WHERE FoodmenuId = " + foodMenuId + "and StoreId = " + storeId + " AND ISDeleted = 0";
                }
                else if (inventoryType == 2)
                {
                    query = " SELECT StockQty from Inventory WHERE IngredientId = " + foodMenuId + "and StoreId = " + storeId + " AND ISDeleted = 0";
                }
                else
                {
                    query = " SELECT StockQty from Inventory WHERE AssetItemId = " + foodMenuId + "and StoreId = " + storeId + " AND ISDeleted = 0";
                }

                result = con.ExecuteScalar<decimal>(query, null, sqltrans, 0, System.Data.CommandType.Text);
            }

            return result;
        }

        public List<InventoryTransferModel> GetViewInventoryTransferById(long id)
        {
            List<InventoryTransferModel> inventoryTransferModels = new List<InventoryTransferModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT IA.Id,IA.InventoryType,IA.FromStoreId,S.StoreName As ToStoreName, IA.ToStoreId ,SS.StoreName AS FromStoreName, " +
                    " IA.ReferenceNumber as ReferenceNo, IA.EntryDate as [Date], IA.EmployeeId, E.LastName + E.FirstName as EmployeeName, " +
                            "IA.Notes FROM [InventoryTransfer] IA LEFT JOIN Employee E ON E.Id = IA.EmployeeId " +
                            "INNER JOIN Store S ON S.Id = IA.ToStoreId " +
                            "INNER JOIN Store SS ON SS.Id = IA.FromStoreId " +
                            "WHERE IA.IsDeleted = 0 AND IA.Id = " + id;
                inventoryTransferModels = con.Query<InventoryTransferModel>(query).AsList();
            }
            return inventoryTransferModels;
        }

        public List<InventoryTransferDetailModel> GetViewInventoryTransferDetail(long id)
        {
            List<InventoryTransferDetailModel> inventoryTransferDetailModels = new List<InventoryTransferDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT IAI.Id AS InventoryTransferId,IAI.IngredientId,I.IngredientName,IAI.FoodMenuId, FM.FoodMenuName,IAI.Qty as Quantity,IAI.ConsumptionStatus as ConsumpationStatus, IAI.CurrentStock,IAI.AssetItemId, AI.AssetItemName, " +
                             " (case when IAI.FoodMenuId is null then (case when IAI.IngredientId is null then UA.UnitName else UI.UnitName end) else UF.UnitName end) as UnitName " +
                             " FROM InventoryTransferDetail IAI " +
                             " INNER JOIN InventoryTransfer IA ON IAI.InventoryTransferId = IA.Id " +
                             " LEFT JOIN Ingredient I ON I.Id = IAI.IngredientId " +
                             " LEFT JOIN FoodMenu FM ON FM.Id = IAI.FoodMenuId " +
                             " LEFT JOIN AssetItem AI ON  AI.Id = IAI.AssetItemId "+
                             " left join Units As UI On UI.Id = I.IngredientUnitId " +
                             " left join Units As UF On UF.Id = FM.UnitsId "+
                             " left join Units As UA On UA.Id = AI.UnitId  "+
                             "WHERE IAI.InventoryTransferId= " + id + " and IA.IsDeleted = 0 and IAI.IsDeleted = 0;";
                inventoryTransferDetailModels = con.Query<InventoryTransferDetailModel>(query).AsList();
            }

            return inventoryTransferDetailModels;
        }
    }
}
