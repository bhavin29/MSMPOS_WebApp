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
    public class CardTerminalRepository : ICardTerminalRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public CardTerminalRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<CardTerminalModel> GetCardTerminalList()
        {
            List<CardTerminalModel> addonsModel = new List<CardTerminalModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT CT.Id as Id,CardTerminalName,OutletId,CT.IsActive as IsActive, OutletName as Outlet " +
                            "FROM CardTerminal CT inner join Outlet O on CT.OutletId = O.Id  WHERE CT.IsDeleted = 0 " +
                            "ORDER BY CardTerminalName ";
                addonsModel = con.Query<CardTerminalModel>(query).ToList();
            }

            return addonsModel;
        }

        public int InsertCardTerminal(CardTerminalModel cardTerminalModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                int MaxId = commonRepository.GetMaxId("CardTerminal");

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO CardTerminal (Id, CardTerminalName," +
                            "OutletId, " +
                            "IsActive)" +
                            "VALUES (" + MaxId + ",@CardTerminalName," +
                            "@OutletId," +
                            "@IsActive); SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, cardTerminalModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int UpdateCardTerminal(CardTerminalModel cardTerminalModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE CardTerminal SET CardTerminalName =@CardTerminalName," +
                            "OutletId = @OutletId, " +
                            "IsActive = @IsActive " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, cardTerminalModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int DeleteCardTerminal(int cardTerminalId)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE CardTerminal SET IsDeleted = 1 WHERE Id = {cardTerminalId};";
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
