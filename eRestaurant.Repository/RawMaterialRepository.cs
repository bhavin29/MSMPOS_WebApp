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
    public class RawMaterialRepository : IRawMaterialRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public RawMaterialRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public int DeleteRawMaterial(int id)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE RawMaterial SET isDeleted= 1,DateDeleted=GetUtcDate(),UserIdDeleted= " + LoginInfo.Userid +" WHERE Id = "+ id;
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

        public List<RawMaterialModel> GetRawMaterialList()
        {
            List<RawMaterialModel> rawMaterialModel = new List<RawMaterialModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT Id,RawMaterialName,Notes,IsActive  FROM RawMaterial WHERE IsDeleted = 0 " +
               "ORDER BY RawMaterialName ";

                rawMaterialModel = con.Query<RawMaterialModel>(query).ToList();

            }

            return rawMaterialModel;
        }

        public int InsertRawMaterial(RawMaterialModel rawMaterialModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("RawMaterial", "RawMaterialName", rawMaterialModel.RawMaterialName, rawMaterialModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                int MaxId = commonRepository.GetMaxId("RawMaterial");

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO RawMaterial (Id,RawMaterialName," +
                            "Notes, " +
                            "IsActive,UserIdInserted,DateInserted,IsDeleted)" +
                            "VALUES (" + MaxId + ",@RawMaterialName," +
                            "@Notes," +
                            "@IsActive,"+LoginInfo.Userid +",GetUtcDate(),0); SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, rawMaterialModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int UpdateRawMaterial(RawMaterialModel rawMaterialModel)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("RawMaterial", "RawMaterialName", rawMaterialModel.RawMaterialName, rawMaterialModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE RawMaterial SET RawMaterialName =@RawMaterialName," +
                            "Notes = @Notes, " +
                            "IsActive = @IsActive, " +
                            "UserIdUpdated =  " +LoginInfo.Userid +
                            ",DateUpdated = GetUtcDate() " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, rawMaterialModel, sqltrans, 0, System.Data.CommandType.Text);

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
