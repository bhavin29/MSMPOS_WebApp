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
                var query = " SELECT IAD.Id,IA.Id as InventoryAlterationId,IA.ReferenceNo,IA.StoreId,S.StoreName,IAD.EntryDate,F.FoodMenuName,IAD.Qty,IAD.Amount FROM InventoryAlteration IA " +
                            " Inner Join Store S On S.Id = IA.StoreId Inner Join InventoryAlterationDetail IAD On IA.Id = IAD.InventoryAlterationId Inner Join FoodMenu F On F.Id = IAD.FoodMenuId " +
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

        public int InsertInventoryAlteration(InventoryAlterationModel inventoryAlterationModel)
        {
            int result = 0;
            int foodMenuResult = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO [dbo].[InventoryAlteration] " +
                             "  ([ReferenceNo] " +
                             " ,[EntryDate] " +
                             " ,[StoreId] " +
                             " ,[UserIdInserted]  " +
                             " ,[DateInserted]   " +
                             " ,[IsDeleted])     " +
                             "   VALUES           " +
                             "  (@ReferenceNo, " +
                              "   GetUtcDate()    " +
                              " ,@StoreId, " +
                             "" + LoginInfo.Userid + "," +
                             "   GetUtcDate(),    " +
                             "   0); SELECT CAST(SCOPE_IDENTITY() as int); ";
                result = con.ExecuteScalar<int>(query, inventoryAlterationModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {

                    foreach (var foodmenu in inventoryAlterationModel.InventoryAlterationDetails)
                    {
                        var queryDetails = "INSERT INTO [dbo].[InventoryAlterationDetail]" +
                                             "  ([InventoryAlterationId] " +
                                             " ,[FoodMenuId] " +
                                             " ,[Qty] " +
                                             " ,[Amount] " +
                                             " ,[EntryDate] " +
                                             " ,[UserIdInserted]" +
                                             " ,[DateInserted]" +
                                              " ,[IsDeleted])   " +
                                              "VALUES           " +
                                              "(" + result + "," +
                                              foodmenu.FoodMenuId + "," +
                                              foodmenu.Qty + "," +
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
