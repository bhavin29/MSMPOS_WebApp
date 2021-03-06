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
    public class TaxRepository : ITaxRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public TaxRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public int DeleteTax(int id)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE Tax SET isDeleted= 1,DateDeleted=GetUtcDate(),UserIdDeleted= " + LoginInfo.Userid + " WHERE Id = " + id;
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

        public List<TaxModel> GetTaxList()
        {
            List<TaxModel> taxModel = new List<TaxModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT Id,TaxName,TaxPercentage,TaxType FROM Tax WHERE IsDeleted = 0 " +
               "ORDER BY TaxName ";

                taxModel = con.Query<TaxModel>(query).ToList();

            }

            return taxModel;
        }

        public int InsertTax(TaxModel taxModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("Tax", "TaxName", taxModel.TaxName, taxModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                int MaxId = commonRepository.GetMaxId("Tax");

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO Tax (Id,TaxName,TaxPercentage,TaxType," +
                            "UserIdInserted,DateInserted,IsDeleted)" +
                            "VALUES (" + MaxId + ",@TaxName,@TaxPercentage,@TaxType," +
                            + LoginInfo.Userid + ",GetUtcDate(),0); SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, taxModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                    string output = commonRepository.SyncTableStatus("Tax");
                }
                else
                {
                    sqltrans.Rollback();
                }
                return result;
            }
        }

        public int UpdateTax(TaxModel taxModel)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("Tax", "TaxName", taxModel.TaxName, taxModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE Tax SET TaxName =@TaxName," +
                            "TaxPercentage = @TaxPercentage, " +
                            "TaxType = @TaxType, " +
                            "UserIdUpdated =  " + LoginInfo.Userid +
                            ",DateUpdated = GetUtcDate() " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, taxModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                    string output = commonRepository.SyncTableStatus("Tax");
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
