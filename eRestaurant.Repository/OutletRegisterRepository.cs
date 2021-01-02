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
    public class OutletRegisterRepository : IOutletRegisterRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public OutletRegisterRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<OutletRegisterModel> GetOutletRegisterList()
        {
            List<OutletRegisterModel> OutletRegisterModel = new List<OutletRegisterModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT " +
                            " ORG.Id,ORG.OutletId,O.OutletName, ORG.UserId,U.Username,ORG.OpenDate,ORG.OpeningBalance, " +
                            " ORG.TotalTransaction,ORG.TotalAmount,ORG.Balance,ORG.CloseDateTime, " +
                            " ORG.ApprovalUserId,UA.username as ApprovalUserNAme, ORG.ApprovalDateTime,ORG.ApprovalAmount,ORG.ApprovalNotes  " +
                            " FROM OutletRegister ORG " +
                            " INNER JOIN Outlet O ON O.Id = ORG.OutletId " +
                            " INNER JOIN[User] U ON U.Id = ORG.UserId " +
                            " left outer JOIN[User] UA ON U.Id = ORG.ApprovalUserId " +
                  "ORDER BY OutletName ";
                OutletRegisterModel = con.Query<OutletRegisterModel>(query).ToList();
            }

            return OutletRegisterModel;
        }

        public int InsertOutletRegister(OutletRegisterModel OutletRegisterModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO OutletRegister" +
                             "(OutletId,UserId,OpenDate,OpeningBalance) " +
                            " VALUES " +
                            "(@OutletId,@UserId,@OpenDate,@OpeningBalance); " +
                            " SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, OutletRegisterModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int UpdateOutletRegister(OutletRegisterModel OutletRegisterModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE OutletRegister SET " +
                            "OpeningBalance=@OpeningBalance, ApprovalUserId=@ApprovalUserId, " +
                            " ApprovalDateTime = @ApprovalDateTime, ApprovalAmount = @ApprovalAmount, " +
                            " ApprovalNotes = @ApprovalNotes " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, OutletRegisterModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int DeleteOutletRegister(int outletRegisterId)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE OutletRegister SET IsDeleted = 1 WHERE Id = {outletRegisterId};";
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
