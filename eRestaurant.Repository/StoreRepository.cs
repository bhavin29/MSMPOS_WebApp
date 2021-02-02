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
    public class StoreRepository :IStoreRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public StoreRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<StoreModel> GetStoreList()
        {
            List<StoreModel> storeModel = new List<StoreModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT Id,StoreName,IsMainStore,Notes,IsActive,IsLock FROM Store WHERE IsDeleted = 0 " +
                            "ORDER BY StoreName ";
                storeModel = con.Query<StoreModel>(query).ToList();
            }

            return storeModel;
        }

        public int InsertStore(StoreModel storeModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                int MaxId = commonRepository.GetMaxId("Store");

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO Store (Id,StoreName, IsMainStore,Notes,IsActive,IsLock) " +
                            "VALUES (" + MaxId + ",@StoreName, @IsMainStore,@Notes,@IsActive,@IsLock);" +
                            " SELECT CAST(SCOPE_IDENTITY() as INT);";

                result = con.Execute(query, storeModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();

                    //CREATE ENTRY INTO INVENTORY WITH ALL FOODMENU WITH STOCKQTY AS 0.00
                    query = " INSERT INTO INVENTORY(STOREID, FOODMENUID, STOCKQTY, USERIDINSERTED, ISDELETED) " +
                            " Select S.Id,FM.Id,0,1,0 from FoodMenu FM Cross join STORE S Where S.Id = "+ MaxId;

                    result = con.Execute(query, storeModel, sqltrans, 0, System.Data.CommandType.Text);
                }
                else
                {
                    sqltrans.Rollback();
                }
            }

            return result;
        }

        public int UpdateStore(StoreModel storeModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = " UPDATE Store SET StoreName =@StoreName," +
                            " Notes=@Notes,IsActive=@IsActive,IsLock=@IsLock " +
                            " WHERE Id = @Id;";
                result = con.Execute(query, storeModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int DeleteStore(int storeId)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE Store SET IsDeleted = 1 WHERE Id = {storeId};";
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



    }
}
