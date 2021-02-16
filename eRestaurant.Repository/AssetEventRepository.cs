﻿using Dapper;
using Microsoft.Extensions.Options;
using RocketPOS.Framework;
using RocketPOS.Interface.Repository;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RocketPOS.Repository
{
    public class AssetEventRepository : IAssetEventRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public AssetEventRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public int DeleteAssetEven(int id)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"update AssetEvent set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where id = " + id + ";" +
                            " update AssetEventItem set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where AssetEventId = " + id + ";" +
                            " update AssetEventFoodmenu set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where AssetEventId = " + id + ";"+
                            " update AssetEventIngredient set IsDeleted = 1,DateDeleted=GetUTCDate(),UserIdDeleted=" + LoginInfo.Userid + " where AssetEventId = " + id + ";";
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

        public AssetEventModel GetAssetEventById(int id)
        {
            AssetEventModel assetEventModel = new AssetEventModel();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " Select Id,ReferenceNo,EventType,EventName,EventDatetime,DateInserted,EventPlace,ContactPersonName,ContactPersonNumber,AllocationDatetime,ReturnDatetime,ClosedDatetime,FoodGrossAmount,FoodDiscountAmount,FoodNetAmount,FoodVatAmount,FoodTaxAmount,IngredientNetAmount,AssetItemNetAmount,Status " +
                            " From AssetEvent Where IsDeleted=0 And Id=" + id;
                assetEventModel = con.QueryFirstOrDefault<AssetEventModel>(query);
            }
            return assetEventModel;
        }

        public List<AssetEventFoodmenuModel> GetAssetEventFoodmenuDetails(int assetEventId)
        {
            List<AssetEventFoodmenuModel> assetEventFoodmenuModel = new List<AssetEventFoodmenuModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " SELECT AEF.Id AS AssetEventFoodmenuId,AEF.AssetEventId,AEF.FoodmenuId,F.FoodMenuName,AEF.SalesPrice,AEF.Qunatity,AEF.FoodVatAmount,AEF.FoodTaxAmount,AEF.TotalPrice,U.UnitName As FoodMenuUnitName  FROM AssetEventFoodmenu AEF " +
                            "  inner join FoodMenu F On F.Id=AEF.FoodmenuId  inner join Units U On U.Id=F.UnitsId Where AEF.IsDeleted=0 And AEF.AssetEventId=" + assetEventId;
                assetEventFoodmenuModel = con.Query<AssetEventFoodmenuModel>(query).AsList();
            }
            return assetEventFoodmenuModel;
        }

        public List<AssetEventItemModel> GetAssetEventItemDetails(int assetEventId)
        {
            List<AssetEventItemModel> assetEventItemModel = new List<AssetEventItemModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " SELECT AEI.Id As AssetEventItemId,AEI.AssetEventId,AEI.AssetItemId,AI.AssetItemName,AEI.StockQty,AEI.EventQty,AEI.AllocatedQty,AEI.ReturnQty,AEI.MissingQty,AEI.CostPrice,AEI.TotalAmount,AEI.MissingNote FROM AssetEventItem AEI " +
                            " Inner Join AssetItem AI ON AI.Id=AEI.AssetItemId Where AEI.IsDeleted=0 And AEI.AssetEventId=" + assetEventId;
                assetEventItemModel = con.Query<AssetEventItemModel>(query).AsList();
            }
            return assetEventItemModel;
        }

        public List<AssetEventViewModel> GetAssetEventList(bool isHistory)
        {
            List<AssetEventViewModel> assetEventList = new List<AssetEventViewModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query=string.Empty;
                if (isHistory)
                {
                     query = " SELECT AE.Id,AE.ReferenceNo,AE.EventName,AE.Status,U.Username FROM AssetEvent  AE  " +
                            " inner join [User] U On U.Id=AE.UserIdInserted where AE.IsDeleted=0 And AE.Status = 4";
                }
                else
                {
                    query = " SELECT AE.Id,AE.ReferenceNo,AE.EventName,AE.Status,U.Username FROM AssetEvent  AE  " +
                            " inner join [User] U On U.Id=AE.UserIdInserted where AE.IsDeleted=0 And AE.Status In (1,2,3) ";
                }
                assetEventList = con.Query<AssetEventViewModel>(query).AsList();
            }
            return assetEventList;
        }

        public List<AssetEventIngredientModel> GetAssetIngredientDetails(int assetEventId)
        {
            List<AssetEventIngredientModel> assetEventIngredientModel = new List<AssetEventIngredientModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select AEI.Id AS AssetEventIngredientId,AEI.IngredientId,I.IngredientName,AEI.StockQty,AEI.EventQty,AEI.ReturnQty,AEI.ActualQty,AEI.CostPrice,AEI.TotalAmount,U.UnitName As IngredientUnitName from AssetEventIngredient AEI " +
                            " Inner Join Ingredient I On I.Id=AEI.IngredientId inner join Units U On U.Id=I.IngredientUnitId Where AEI.IsDeleted=0 And AEI.AssetEventId=" + assetEventId;
                assetEventIngredientModel = con.Query<AssetEventIngredientModel>(query).AsList();
            }
            return assetEventIngredientModel;
        }

        public decimal GetAssetItemPriceById(int id)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " Select CostPrice From AssetItem Where IsDeleted=0 And Id=" + id;
                return con.ExecuteScalar<decimal>(query);
            }
        }

        public AssetFoodMenuPriceDetail GetFoodMenuPriceTaxDetailById(int id)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select CateringPrice as SalesPrice,FoodVatTaxId,T.TaxPercentage from FoodMenu F inner join Tax T On F.FoodVatTaxId=T.Id where F.IsDeleted=0 And F.id=" + id;
                return con.Query<AssetFoodMenuPriceDetail>(query).ToList().FirstOrDefault();
            }
        }

        public decimal GetIngredientPriceById(int id)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " Select SalesPrice From Ingredient Where IsDeleted=0 And Id=" + id;
                return con.ExecuteScalar<decimal>(query);
            }
        }

        public int InsertAssetEvent(AssetEventModel assetEventModel)
        {
            int result = 0;
            int foodMenuResult = 0, itemResult = 0, ingredientResult = 0; 
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO [dbo].[AssetEvent] " +
                             " ([ReferenceNo] " +
                             " ,[EventType] " +
                             " ,[EventName] " +
                             " ,[EventDatetime] " +
                             " ,[EventPlace] " +
                             " ,[ContactPersonName] " +
                             " ,[ContactPersonNumber] " +
                             " ,[FoodGrossAmount] " +
                             " ,[FoodDiscountAmount] " +
                             " ,[FoodNetAmount] " +
                             " ,[FoodVatAmount] " +
                             " ,[FoodTaxAmount] " +
                             " ,[IngredientNetAmount] " +
                             " ,[AssetItemNetAmount] " +
                             " ,[Status] " +
                             "  ,[UserIdInserted]  " +
                             "  ,[DateInserted]   " +
                             "  ,[IsDeleted])     " +
                             "   VALUES           " +
                             "  ( @ReferenceNo " +
                             " ,@EventType " +
                             " ,@EventName " +
                             " ,@EventDatetime " +
                             " ,@EventPlace " +
                             " ,@ContactPersonName " +
                             " ,@ContactPersonNumber " +
                             " ,@FoodGrossAmount " +
                             " ,@FoodDiscountAmount " +
                             " ,@FoodNetAmount " +
                             " ,@FoodVatAmount " +
                             " ,@FoodTaxAmount " +
                             " ,@IngredientNetAmount " +
                             " ,@AssetItemNetAmount " +
                             " ,@Status, " +
                             "" + LoginInfo.Userid + "," +
                             "   GetUtcDate(),    " +
                             "   0); SELECT CAST(SCOPE_IDENTITY() as int); ";
                result = con.ExecuteScalar<int>(query, assetEventModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {

                    if (assetEventModel.assetEventItemModels != null)
                    {
                        foreach (var assetItem in assetEventModel.assetEventItemModels)
                        {
                            var queryDetails = "INSERT INTO [dbo].[AssetEventItem]" +
                                                 "  ([AssetEventId] " +
                                                  " ,[AssetItemId] " +
                                                 " ,[StockQty] " +
                                                 " ,[EventQty] " +
                                                 " ,[AllocatedQty] " +
                                                 " ,[ReturnQty] " +
                                                 " ,[MissingQty] " +
                                                 " ,[CostPrice] " +
                                                 " ,[TotalAmount] " +
                                                 " ,[MissingNote] " +
                                                 " ,[UserIdInserted]" +
                                                 " ,[DateInserted]" +
                                                  " ,[IsDeleted])   " +
                                                  "VALUES           " +
                                                  "(" + result + "," +
                                                  assetItem.AssetItemId + "," +
                                                  assetItem.StockQty + "," +
                                                  assetItem.EventQty + "," +
                                                  assetItem.AllocatedQty + "," +
                                                  assetItem.ReturnQty + "," +
                                                  assetItem.MissingQty + "," +
                                                  assetItem.CostPrice + "," +
                                                  assetItem.TotalAmount + ",'" +
                                                  assetItem.MissingNote + "'," +
                                        LoginInfo.Userid + ",GetUtcDate(),0);";
                            itemResult = con.Execute(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }

                    if (assetEventModel.assetEventFoodmenuModels != null)
                    {
                        foreach (var assetFood in assetEventModel.assetEventFoodmenuModels)
                        {

                            var queryDetails = "INSERT INTO [dbo].[AssetEventFoodmenu]" +
                                                 "  ([AssetEventId] " +
                                                 " ,[FoodmenuId] " +
                                                 " ,[SalesPrice] " +
                                                 " ,[Qunatity] " +
                                                 " ,[FoodVatAmount] " +
                                                 " ,[FoodTaxAmount] " +
                                                 " ,[TotalPrice] " +
                                                 " ,[UserIdInserted]" +
                                                 " ,[DateInserted]" +
                                                  " ,[IsDeleted])   " +
                                                  "VALUES           " +
                                                  "(" + result + "," +
                                                  assetFood.FoodMenuId + "," +
                                                  assetFood.SalesPrice + "," +
                                                   assetFood.Qunatity + "," +
                                                    assetFood.FoodVatAmount + "," +
                                                     assetFood.FoodTaxAmount + "," +
                                                   assetFood.TotalPrice + "," +
                                        LoginInfo.Userid + ",GetUtcDate(),0);";
                            foodMenuResult = con.Execute(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }

                    if (assetEventModel.assetEventIngredientModels != null)
                    {
                        foreach (var assetItem in assetEventModel.assetEventIngredientModels)
                        {
                            var queryDetails = "INSERT INTO [dbo].[AssetEventIngredient]" +
                                                 "  ([AssetEventId] " +
                                                  " ,[IngredientId] " +
                                                 " ,[StockQty] " +
                                                 " ,[EventQty] " +
                                                 " ,[ReturnQty] " +
                                                 " ,[ActualQty] " +
                                                 " ,[CostPrice] " +
                                                 " ,[TotalAmount] " +
                                                 " ,[UserIdInserted]" +
                                                 " ,[DateInserted]" +
                                                  " ,[IsDeleted])   " +
                                                  "VALUES           " +
                                                  "(" + result + "," +
                                                  assetItem.IngredientId + "," +
                                                  assetItem.StockQty + "," +
                                                  assetItem.EventQty + "," +
                                                  assetItem.ReturnQty + "," +
                                                  assetItem.ActualQty + "," +
                                                  assetItem.CostPrice + "," +
                                                  assetItem.TotalAmount + "," +
                                        LoginInfo.Userid + ",GetUtcDate(),0);";
                            ingredientResult = con.Execute(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }

                    if (foodMenuResult > 0 && itemResult > 0 && ingredientResult > 0)
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

        public string ReferenceNumberAssetEvent()
        {
            string result = string.Empty;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"SELECT ISNULL(MAX(convert(int,ReferenceNo)),0) + 1  FROM AssetEvent where ISDeleted=0;";
                result = con.ExecuteScalar<string>(query, null, sqltrans, 0, System.Data.CommandType.Text);
                if (!string.IsNullOrEmpty(result))
                {
                    sqltrans.Commit();
                }
                else
                { sqltrans.Rollback(); }
            }
            return result;
        }

        public int UpdateAssetEvent(AssetEventModel assetEventModel)
        {
            int result = 0, deleteFoodMenuResult = 0, deleteItemResult = 0, foodmenudetails = 0, itemdetails = 0, ingredientdetails = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "Update [dbo].[AssetEvent] set " +
                            " EventType = @EventType " +
                            ",EventName = @EventName " +
                            ",EventDatetime = @EventDatetime " +
                            ",EventPlace = @EventPlace " +
                            ",ContactPersonName = @ContactPersonName " +
                            ",ContactPersonNumber = @ContactPersonNumber " +
                            ",FoodGrossAmount = @FoodGrossAmount " +
                            ",FoodDiscountAmount = @FoodDiscountAmount " +
                            ",FoodVatAmount = @FoodVatAmount " +
                            ",FoodTaxAmount = @FoodTaxAmount " +
                            ",IngredientNetAmount = @IngredientNetAmount " +
                            ",AssetItemNetAmount = @AssetItemNetAmount " +
                            ",FoodNetAmount = @FoodNetAmount ";

                if (assetEventModel.Status==2)
                {
                    query += ",AllocationDatetime = GetUtcDate() ";
                }
                if (assetEventModel.Status == 3)
                {
                    query += ",ReturnDatetime = GetUtcDate() ";
                }
                if (assetEventModel.Status == 4)
                {
                    query += ",ClosedDatetime = GetUtcDate() ";
                }

                query += ",Status = @Status " +
                            "  ,[UserIdUpdated] = " + LoginInfo.Userid + " " +
                            "  ,[DateUpdated]  = GetUtcDate()  where id= " + assetEventModel.Id + ";";
                result = con.Execute(query, assetEventModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {

                    if (assetEventModel.AssetEventItemDeletedId != null)
                    {
                        foreach (var item in assetEventModel.AssetEventItemDeletedId)
                        {
                            var deleteQuery = $"update AssetEventItem set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                            deleteItemResult = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }

                    if (assetEventModel.AssetEventFoodmenuDeletedId != null)
                    {
                        foreach (var item in assetEventModel.AssetEventFoodmenuDeletedId)
                        {
                            var deleteQuery = $"update AssetEventFoodmenu set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                            deleteFoodMenuResult = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }

                    if (assetEventModel.AssetEventIngredientDeletedId != null)
                    {
                        foreach (var item in assetEventModel.AssetEventIngredientDeletedId)
                        {
                            var deleteQuery = $"update AssetEventIngredient set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                            deleteFoodMenuResult = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }

                    foreach (var item in assetEventModel.assetEventItemModels)
                    {
                        var queryDetails = string.Empty;
                        if (item.AssetEventItemId > 0)
                        {
                            queryDetails = "Update [dbo].[AssetEventItem] set " +
                                             "[AssetItemId]     = " + item.AssetItemId +
                                             ",[StockQty]     = " + item.StockQty +
                                             ",[EventQty]     = " + item.EventQty +
                                             ",[AllocatedQty]     = " + item.AllocatedQty +
                                             ",[ReturnQty]     = " + item.ReturnQty +
                                             ",[MissingQty]     = " + item.MissingQty +
                                             ",[CostPrice]     = " + item.CostPrice +
                                             ",[TotalAmount]     = " + item.TotalAmount +
                                             ",[MissingNote]     = '" + item.MissingNote +
                                             "',[UserIdUpdated] = " + LoginInfo.Userid + "," +
                                             " [DateUpdated] = GetUTCDate() " +
                                             " where id = " + item.AssetEventItemId + ";";
                        }
                        else
                        {
                            queryDetails = "INSERT INTO [dbo].[AssetEventItem]" +
                                                 "  ([AssetEventId] " +
                                                  " ,[AssetItemId] " +
                                                 " ,[StockQty] " +
                                                 " ,[EventQty] " +
                                                 " ,[AllocatedQty] " +
                                                 " ,[ReturnQty] " +
                                                 " ,[MissingQty] " +
                                                 " ,[CostPrice] " +
                                                 " ,[TotalAmount] " +
                                                 " ,[MissingNote] " +
                                                 " ,[UserIdInserted]" +
                                                 " ,[DateInserted]" +
                                                  " ,[IsDeleted])   " +
                                                  "VALUES           " +
                                                  "(" + assetEventModel.Id + "," +
                                                  item.AssetItemId + "," +
                                                  item.StockQty + "," +
                                                   item.EventQty + "," +
                                                    item.AllocatedQty + "," +
                                                     item.ReturnQty + "," +
                                                      item.MissingQty + "," +
                                                       item.CostPrice + "," +
                                                        item.TotalAmount + ",'" +
                                                      item.MissingNote + "'," +
                                        LoginInfo.Userid + ",GetUtcDate(),0);";
                        }
                        itemdetails = con.Execute(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                    }

                    foreach (var item in assetEventModel.assetEventFoodmenuModels)
                    {
                        var queryDetails = string.Empty;
                        if (item.AssetEventFoodmenuId > 0)
                        {
                            queryDetails = "Update [dbo].[AssetEventFoodmenu] set " +
                                             "[FoodmenuId]		  	 = " + item.FoodMenuId +
                                             ",[SalesPrice]     = " + item.SalesPrice +
                                             ",[Qunatity]     = " + item.Qunatity +
                                             ",[FoodVatAmount]     = " + item.FoodVatAmount +
                                             ",[FoodTaxAmount]     = " + item.FoodTaxAmount +
                                             ",[TotalPrice]     = " + item.TotalPrice +
                                             " ,[UserIdUpdated] = " + LoginInfo.Userid + "," +
                                             " [DateUpdated] = GetUTCDate() " +
                                             " where id = " + item.AssetEventFoodmenuId + ";";
                        }
                        else
                        {
                             queryDetails = "INSERT INTO [dbo].[AssetEventFoodmenu]" +
                                                "  ([AssetEventId] " +
                                                " ,[FoodmenuId] " +
                                                " ,[SalesPrice] " +
                                                " ,[Qunatity] " +
                                                " ,[FoodVatAmount] " +
                                                " ,[FoodTaxAmount] " +
                                                " ,[TotalPrice] " +
                                                " ,[UserIdInserted]" +
                                                " ,[DateInserted]" +
                                                 " ,[IsDeleted])   " +
                                                 "VALUES           " +
                                                 "(" + assetEventModel.Id + "," +
                                                 item.FoodMenuId + "," +
                                                 item.SalesPrice + "," +
                                                  item.Qunatity + "," +
                                                    item.FoodVatAmount + "," +
                                                      item.FoodTaxAmount + "," +
                                                  item.TotalPrice + "," +
                                       LoginInfo.Userid + ",GetUtcDate(),0);";
                        }
                        foodmenudetails = con.Execute(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                    }

                    foreach (var item in assetEventModel.assetEventIngredientModels)
                    {
                        var queryDetails = string.Empty;
                        if (item.AssetEventIngredientId > 0)
                        {
                            queryDetails = "Update [dbo].[AssetEventIngredient] set " +
                                             "[IngredientId]     = " + item.IngredientId +
                                             ",[StockQty]     = " + item.StockQty +
                                             ",[EventQty]     = " + item.EventQty +
                                             ",[ReturnQty]     = " + item.ReturnQty +
                                             ",[ActualQty]     = " + item.ActualQty +
                                             ",[CostPrice]     = " + item.CostPrice +
                                             ",[TotalAmount]     = " + item.TotalAmount +
                                             ",[UserIdUpdated] = " + LoginInfo.Userid + "," +
                                             " [DateUpdated] = GetUTCDate() " +
                                             " where id = " + item.AssetEventIngredientId + ";";
                        }
                        else
                        {
                            queryDetails = "INSERT INTO [dbo].[AssetEventIngredient]" +
                                                 "  ([AssetEventId] " +
                                                  " ,[IngredientId] " +
                                                 " ,[StockQty] " +
                                                 " ,[EventQty] " +
                                                 " ,[ReturnQty] " +
                                                 " ,[ActualQty] " +
                                                 " ,[CostPrice] " +
                                                 " ,[TotalAmount] " +
                                                 " ,[UserIdInserted]" +
                                                 " ,[DateInserted]" +
                                                  " ,[IsDeleted])   " +
                                                  "VALUES           " +
                                                  "(" + assetEventModel.Id + "," +
                                                  item.IngredientId + "," +
                                                  item.StockQty + "," +
                                                  item.EventQty + "," +
                                                  item.ReturnQty + "," +
                                                  item.ActualQty + "," +
                                                  item.CostPrice + "," +
                                                  item.TotalAmount + "," +
                                        LoginInfo.Userid + ",GetUtcDate(),0);";
                        }
                        ingredientdetails = con.Execute(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                    }

                    if (foodmenudetails > 0 && itemdetails > 0 && ingredientdetails > 0)
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
