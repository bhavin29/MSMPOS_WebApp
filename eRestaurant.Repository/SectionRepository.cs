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
    public class SectionRepository : ISectionRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public SectionRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public int DeleteSection(int id)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE Section SET isDeleted= 1,DateDeleted=GetUtcDate(),UserIdDeleted= " + LoginInfo.Userid + " WHERE Id = " + id;
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

        public List<SectionModel> GetSectionList()
        {
            List<SectionModel> SectionModel = new List<SectionModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT Id,SectionName,Notes,IsActive  FROM Section WHERE IsDeleted = 0 " +
               "ORDER BY SectionName ";

                SectionModel = con.Query<SectionModel>(query).ToList();

            }

            return SectionModel;
        }

        public int InsertSection(SectionModel SectionModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("Section", "SectionName", SectionModel.SectionName, SectionModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                int MaxId = commonRepository.GetMaxId("Section");

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO Section (Id,SectionName," +
                            "Notes, " +
                            "IsActive,UserIdInserted,DateInserted,IsDeleted)" +
                            "VALUES (" + MaxId + ",@SectionName," +
                            "@Notes," +
                            "@IsActive," + LoginInfo.Userid + ",GetUtcDate(),0); SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, SectionModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int UpdateSection(SectionModel SectionModel)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("Section", "SectionName", SectionModel.SectionName, SectionModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE Section SET SectionName =@SectionName," +
                            "Notes = @Notes, " +
                            "IsActive = @IsActive, " +
                            "UserIdUpdated =  " + LoginInfo.Userid +
                            ",DateUpdated = GetUtcDate() " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, SectionModel, sqltrans, 0, System.Data.CommandType.Text);

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
