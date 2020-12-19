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
    public class OutletRepository : IOutletRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public OutletRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<OutletModel> GetOutletList()
        {
            List<OutletModel> outletModel = new List<OutletModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT O.Id, StoreId,Storename, OutletName, OutletAddress1, OutletAddress2, OutletPhone, OutletEmail, InvoiceHeader,  " +
                            "InvoiceFooter, IsCollectTax, IsPreorPostPayment, O.IsActive, O.IsLock from Outlet O inner join Store S on O.StoreId = S.Id WHERE O.IsDeleted = 0  " +
                            "ORDER BY OutletName ";
                outletModel = con.Query<OutletModel>(query).ToList();
            }

            return outletModel;
        }

        public int InsertOutlet(OutletModel outletModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO Outlet"+
                             "(StoreId, OutletName, OutletAddress1, OutletAddress2, OutletPhone, OutletEmail," +
                             " InvoiceHeader, InvoiceFooter, IsCollectTax, IsPreorPostPayment, IsActive, IsLock)" +
                            "VALUES " +
                            "( @StoreId, @OutletName, @OutletAddress1, @OutletAddress2, @OutletPhone, @OutletEmail, "+
                            "@InvoiceHeader, @InvoiceFooter, @IsCollectTax, @IsPreorPostPayment, @IsActive, @IsLock); "+
                            " SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, outletModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int UpdateOutlet(OutletModel outletModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE Outlet SET " +
                   "StoreId=@StoreId, OutletName=@OutletName, OutletAddress1=@OutletAddress1, OutletAddress2=@OutletAddress2, " +
                   "OutletPhone=@OutletPhone, OutletEmail=@OutletEmail, InvoiceHeader=@InvoiceHeader, InvoiceFooter=@InvoiceFooter, " +
                   "IsCollectTax=@IsCollectTax, IsPreorPostPayment=@IsPreorPostPayment, IsActive=@IsActive, IsLock=@IsLock " +
                    "WHERE Id = @Id;";
                result = con.Execute(query, outletModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int DeleteOutlet (int addonsId)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE Outlet SET IsDeleted = 1 WHERE Id = {addonsId};";
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
