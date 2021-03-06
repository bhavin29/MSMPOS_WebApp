using Dapper;
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
    public class AssetItemRepository : IAssetItemRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;
        public AssetItemRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }
        public int DeleteAssetItem(int id)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE AssetItem SET IsDeleted = 1,DateDeleted=GetUtcDate(),UserIdDeleted=" + LoginInfo.Userid + " WHERE Id = " + id;
                result = con.Execute(query, null, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
            }
            return result;
        }

        public AssetItemModel GetAssetItemById(int id)
        {
            AssetItemModel assetItemModel = new AssetItemModel();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select AI.Id,AssetItemName,ShortName,Code,Brandname,Model,Picture,AI.Notes,CostPrice,AI.UnitId,U.UnitName from AssetItem AI " +
                            " Inner Join Units U ON U.Id = AI.UnitId " +
                            " Where AI.IsDeleted = 0 And AI.Id = "+id;
                assetItemModel = con.Query<AssetItemModel>(query).FirstOrDefault();
            }
            return assetItemModel;
        }

        public List<AssetItemModel> GetAssetItemList()
        {
            List<AssetItemModel> assetItemModel = new List<AssetItemModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " select AI.Id,AssetItemName,ShortName,Code,Brandname,Model,Picture,AI.Notes,CostPrice,AI.UnitId,U.UnitName from AssetItem AI " +
                            " Inner Join Units U ON U.Id = AI.UnitId " +
                            " Where AI.IsDeleted = 0 ";
                assetItemModel = con.Query<AssetItemModel>(query).ToList();
            }
            return assetItemModel;
        }

        public int InsertAssetItem(AssetItemModel assetItemModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("AssetItem", "AssetItemName", assetItemModel.AssetItemName, assetItemModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                int MaxId = commonRepository.GetMaxId("AssetItem");

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO AssetItem " +
                    "(AssetItemName, ShortName, Code, Brandname, Model, Notes,CostPrice,UnitId," +
                    " UserIdInserted,  DateInserted,IsDeleted) " +
                    "Values " +
                    "(@AssetItemName, @ShortName,@Code, @Brandname, @Model,@Notes,@CostPrice,@UnitId," +
                  LoginInfo.Userid + ",GetUtcDate(),0);" +
                    " SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, assetItemModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                    //CREATE ENTRY INTO INVETORY AS STOCK 0.00
                    query = " INSERT INTO INVENTORY (STOREID,AssetItemId,STOCKQTY,USERIDINSERTED,ISDELETED)" +
                            " Select S.ID as StoreId,FM.Id,0,1,0 from AssetItem FM CROSS JOIN STORE S " +
                            " WHERE FM.ID =" + MaxId;
                    result = con.Execute(query, assetItemModel, sqltrans, 0, System.Data.CommandType.Text);

                    string output = commonRepository.SyncTableStatus("AssetItem");
                    output = commonRepository.SyncTableStatus("Inventory");
                }
                else
                {
                    sqltrans.Rollback();
                }
            }

            return result;
        }

        public int UpdateAssetItem(AssetItemModel assetItemModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("AssetItem", "AssetItemName", assetItemModel.AssetItemName, assetItemModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE AssetItem SET " +
                     "AssetItemName=@AssetItemName, " +
                     "ShortName=@ShortName, " +
                     "Code=@Code, " +
                     "CostPrice=@CostPrice, " +
                     " Brandname = @Brandname," +
                     " Model =@Model ," +
                     "Notes=@Notes," +
                     "UnitId=@UnitId, " +
                     "[UserIdUpdated] = " + LoginInfo.Userid + " " +
                     ",[DateUpdated]  = GetUtcDate() WHERE Id = @Id;";
                result = con.Execute(query, assetItemModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                    string output = commonRepository.SyncTableStatus("AssetItem");
                }
                else
                {
                    sqltrans.Rollback();
                }
                return result;
            }
        }
    }
}
