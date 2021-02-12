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
    public class TablesRepository : ITablesRpository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public TablesRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<TablesModel> GetTablesList()
        {
            List<TablesModel> TablesModel = new List<TablesModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = " Select T.Id,TableName,OutletId,OutletName,PersonCapacity,TableIcon,Status,T.IsActive FROM Tables T INNER JOIN Outlet O ON T.OutletId = O.Id WHERE T.IsDeleted=0" +
                            "ORDER BY T.TableName ";
                TablesModel = con.Query<TablesModel>(query).ToList();
            }

            return TablesModel;
        }

        public int InsertTables(TablesModel TablesModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                TablesModel.Status = (TablesModel.Status == 0) ? null : TablesModel.Status;  

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO Tables (TableName,OutletId,PersonCapacity,TableIcon,Status,IsActive)" +
                            "VALUES" +
                            "(@TableName,@OutletId,@PersonCapacity,@TableIcon,@Status,@IsActive);" +
                            " SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, TablesModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int UpdateTables(TablesModel TablesModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE Tables SET TableName=@TableName,OutletId=@OutletId," +
                            "PersonCapacity=@PersonCapacity,TableIcon=@TableIcon,Status=@Status,IsActive=@IsActive " +
                           "WHERE Id = @Id;";
                result = con.Execute(query, TablesModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int DeleteTables(int TablesId)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE Tables SET IsDeleted = 1 WHERE Id = {TablesId};";
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
