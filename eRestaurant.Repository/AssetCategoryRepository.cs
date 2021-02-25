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
    public class AssetCategoryRepository : IAssetCategoryRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public AssetCategoryRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }
        public int DeleteAssetCategory(int id)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE AssetCategory SET isDeleted= 1,DateDeleted=GetUtcDate(),UserIdDeleted="+LoginInfo.Userid +" WHERE Id = " + id;
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

        public List<AssetCategoryModel> GetAssetCategoryList()
        {
            List<AssetCategoryModel> assetCategoryModel = new List<AssetCategoryModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select Id,AssetCategoryName,Notes from AssetCategory Where IsDeleted=0 " +
                              "ORDER BY AssetCategoryName ";
                assetCategoryModel = con.Query<AssetCategoryModel>(query).ToList();
            }
            return assetCategoryModel;
        }

        public int InsertAssetCategory(AssetCategoryModel assetCategoryModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("AssetCategory", "AssetCategoryName", assetCategoryModel.AssetCategoryName, assetCategoryModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                int MaxId = commonRepository.GetMaxId("AssetCategory");
                
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO AssetCategory (Id, AssetCategoryName," +
                            "Notes, " +
                            "UserIdInserted,DateInserted,IsDeleted)" +
                            "VALUES (" + MaxId + ",@AssetCategoryName," +
                            "@Notes," +
                            LoginInfo.Userid +",GetUtcDate(),0); SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, assetCategoryModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
                return result;
            }
        }

        public int UpdateAssetCategory(AssetCategoryModel assetCategoryModel)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("AssetCategory", "AssetCategoryName", assetCategoryModel.AssetCategoryName, assetCategoryModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE AssetCategory SET AssetCategoryName =@AssetCategoryName," +
                            "Notes = @Notes, " +
                            "UserIdUpdated =  " + LoginInfo.Userid + ",DateUpdated=GetUtcDate() " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, assetCategoryModel, sqltrans, 0, System.Data.CommandType.Text);

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
    }
}
