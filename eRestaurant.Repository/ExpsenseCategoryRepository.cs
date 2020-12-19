using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;
using RocketPOS.Interface.Repository;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using Dapper;
using System.Linq;

namespace RocketPOS.Repository
{
    public class ExpsenseCategoryRepository : IExpsenseCategoryRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public ExpsenseCategoryRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public int DeleteExpsenseCategory(int expenseCategoryId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE ExpenseCategory SET isDeleted= 1 WHERE Id = {expenseCategoryId}";
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

        public List<ExpsenseCategoryModel> GetExpsenseCategoryList()
        {
            List<ExpsenseCategoryModel> expenseCategoryModel = new List<ExpsenseCategoryModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT Id,ExpenseCategory,Notes,IsActive  FROM ExpenseCategory WHERE IsDeleted = 0 " +
               "ORDER BY ExpenseCategory ";

                expenseCategoryModel = con.Query<ExpsenseCategoryModel>(query).ToList();

            }

            return expenseCategoryModel;
        }

        public int InsertExpsenseCategory(ExpsenseCategoryModel expenseCAtegoryModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO ExpenseCategory (ExpenseCategory," +
                            "Notes, " +
                            "IsActive)" +
                            "VALUES (@ExpenseCategory," +
                            "@Notes," +
                            "@IsActive); SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, expenseCAtegoryModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int UpdateExpsenseCategory(ExpsenseCategoryModel expenseCAtegoryModel)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE ExpenseCategory SET ExpenseCategory =@ExpenseCategory," +
                            "Notes = @Notes, " +
                            "IsActive = @IsActive " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, expenseCAtegoryModel, sqltrans, 0, System.Data.CommandType.Text);

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
