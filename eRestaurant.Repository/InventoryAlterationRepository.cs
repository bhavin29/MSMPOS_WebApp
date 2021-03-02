using Dapper;
using Microsoft.Extensions.Options;
using RocketPOS.Framework;
using RocketPOS.Interface.Repository;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RocketPOS.Repository
{
    public class InventoryAlterationRepository: IInventoryAlterationRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;
        public InventoryAlterationRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<InventoryAlterationViewListModel> GetInventoryAlterationList(int storeId, DateTime fromDate, DateTime toDate, int foodMenuId)
        {
            List<InventoryAlterationViewListModel> inventoryAlterationViewList = new List<InventoryAlterationViewListModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " SELECT IAD.Id,IA.Id as InventoryAlterationId,IA.ReferenceNo,IA.StoreId,S.StoreName,IAD.EntryDate," +
                            " (case when IAD.FoodMenuId is null then (case when IAD.IngredientId is null then AI.AssetItemName else IG.IngredientName end) else f.FoodMenuName end) as FoodMenuName, " +
                            " IAD.Qty,IAD.InventoryStockQty,IAD.Amount FROM InventoryAlteration IA " +
                            " Inner Join Store S On S.Id = IA.StoreId " +
                            " Inner Join InventoryAlterationDetail IAD On IA.Id = IAD.InventoryAlterationId " +
                            " Left Join FoodMenu F On F.Id = IAD.FoodMenuId " +
                            " Left Join Ingredient IG On IG.Id = IAD.IngredientId " +
                            " left join AssetItem as AI on IAD.AssetItemId = AI.Id "+
                            " Where IA.IsDeleted = 0";

                if (storeId != 0)
                {
                    query += " And IA.StoreId = " + storeId;
                }

                if (foodMenuId != 0)
                {
                    query += " And IAD.FoodMenuId = " + foodMenuId;
                }
                query += " AND Convert(Date, IAD.EntryDate, 103)  between Convert(Date, '" + fromDate + "', 103)  and Convert(Date, '" + toDate + "' , 103)  ";
                inventoryAlterationViewList = con.Query<InventoryAlterationViewListModel>(query).AsList();
            }
            return inventoryAlterationViewList;
        }

        public decimal GetInventoryStockQty(int storeId, int foodMenuId)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " Select StockQty from Inventory where Storeid ="+ storeId + " and Foodmenuid = " + foodMenuId;
                return con.ExecuteScalar<decimal>(query);
            }
        }

        public decimal GetInventoryStockQtyForIngredient(int storeId, int ingredientId)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " Select StockQty from Inventory where Storeid =" + storeId + " and IngredientId = " + ingredientId;
                return con.ExecuteScalar<decimal>(query);
            }
        }

        public List<InventoryAlterationModel> GetViewInventoryAlterationById(long invAltId)
        {
            List<InventoryAlterationModel> inventoryAlterationModel = new List<InventoryAlterationModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "Select IA.Id,IA.ReferenceNo,IA.StoreId,IA.EntryDate,IA.Notes,S.StoreName,IA.InventoryType from InventoryAlteration IA INNER JOIN Store S ON S.Id = IA.StoreId " +
                              "WHERE IA.IsDeleted = 0 AND IA.Id = " + invAltId;
                inventoryAlterationModel = con.Query<InventoryAlterationModel>(query).AsList();
            }
            return inventoryAlterationModel;
        }

        public List<InventoryAlterationDetailModel> GetViewInventoryAlterationDetail(long invAltId)
        {
            List<InventoryAlterationDetailModel> inventoryAlterationDetailModel = new List<InventoryAlterationDetailModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "Select IAD.InventoryAlterationId,IAD.Id,IAD.InventoryStockQty,IAD.Qty,IAD.Amount,IAD.EntryDate, " +
                             "  (case when IAD.FoodMenuId is null then (case when IAD.IngredientId is null then AI.AssetItemName else IG.IngredientName end) else f.FoodMenuName end) as FoodMenuName , " +
                             " (case when IAD.FoodMenuId is null then (case when IAD.IngredientId is null then UA.UnitName else UI.UnitName end) else UF.UnitName end) as UnitName  " +
                             " from InventoryAlterationDetail IAD " +
                             " Inner Join InventoryAlteration IA ON IA.Id=IAD.InventoryAlterationId  Left Join FoodMenu F On F.Id = IAD.FoodMenuId   Left Join Ingredient IG On IG.Id = IAD.IngredientId " +
                             " left join AssetItem as AI on IAD.AssetItemId = AI.Id  left join Units As UI On UI.Id = IG.IngredientUnitId  left join Units As UF On UF.Id = F.UnitsId   " +
                             " left join Units As UA On UA.Id = AI.UnitId  Where IAD.IsDeleted=0 And IAD.InventoryAlterationId =  " + invAltId;
                inventoryAlterationDetailModel = con.Query<InventoryAlterationDetailModel>(query).AsList();
            }

            return inventoryAlterationDetailModel;
        }

        public int InsertInventoryAlteration(InventoryAlterationModel inventoryAlterationModel)
        {
            int result = 0;
            int foodMenuResult = 0;
            string foodMenuId = "NULL", ingredientId = "NULL", assetItemId = "NULL";
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO [dbo].[InventoryAlteration] " +
                             "  ([ReferenceNo] " +
                             " ,[EntryDate] " +
                             " ,[StoreId] " +
                             " ,[Notes] " +
                             " ,[InventoryType] " +
                             " ,[UserIdInserted]  " +
                             " ,[DateInserted]   " +
                             " ,[IsDeleted])     " +
                             "   VALUES           " +
                             "  (@ReferenceNo, " +
                             "   GetUtcDate()    " +
                             "  ,@StoreId, " +
                             "  @Notes, " +
                              "  @InventoryType, " +
                             "" + LoginInfo.Userid + "," +
                             "   GetUtcDate(),    " +
                             "   0); SELECT CAST(SCOPE_IDENTITY() as int); ";
                result = con.ExecuteScalar<int>(query, inventoryAlterationModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {

                    foreach (var foodmenu in inventoryAlterationModel.InventoryAlterationDetails)
                    {
                        if (foodmenu.IngredientId == 0)
                        {
                            ingredientId = "NULL";
                        }
                        else
                        {
                            ingredientId = foodmenu.IngredientId.ToString();
                        }

                        if (foodmenu.AssetItemId == 0)
                        {
                            assetItemId = "NULL";
                        }
                        else
                        {
                            assetItemId = foodmenu.AssetItemId.ToString();
                        }

                        if (foodmenu.FoodMenuId == 0)
                        {
                            foodMenuId = "NULL";
                        }
                        else
                        {
                            foodMenuId = foodmenu.FoodMenuId.ToString();
                        }
                            var queryDetails = "INSERT INTO [dbo].[InventoryAlterationDetail]" +
                                             "  ([InventoryAlterationId] " +
                                             " ,[FoodMenuId] " +
                                             " ,[IngredientId] " +
                                             " ,[AssetItemId] " +
                                             " ,[Qty] " +
                                             " ,[InventoryStockQty] " +
                                             " ,[Amount] " +
                                             " ,[EntryDate] " +
                                             " ,[UserIdInserted]" +
                                             " ,[DateInserted]" +
                                              " ,[IsDeleted])   " +
                                              "VALUES           " +
                                              "(" + result + "," +
                                              foodMenuId + "," +
                                              ingredientId + "," +
                                                assetItemId + "," +
                                              foodmenu.Qty + "," +
                                              foodmenu.InventoryStockQty + "," +
                                              foodmenu.Amount + "," +
                                              " GetUtcDate()" + "," +
                                    LoginInfo.Userid + ",GetUtcDate(),0);";
                        foodMenuResult = con.Execute(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                    }

                    if (foodMenuResult > 0)
                    {
                        sqltrans.Commit();
                        CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                        string sResult = commonRepository.InventoryPush("PhysicalStock", result);
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

        public string ReferenceNumberInventoryAlteration()
        {
            string result = string.Empty;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"SELECT ISNULL(MAX(convert(int,ReferenceNo)),0) + 1 FROM InventoryAlteration where IsDeleted=0;";
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
    }
}
