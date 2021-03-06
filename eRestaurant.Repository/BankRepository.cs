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
    public class BankRepository :IBankRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public BankRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public int DeleteBank(int bankId)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE Bank SET IsDeleted = 1 WHERE Id = {bankId};";
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

        public List<BankModel> GetBankList()
        {
            List<BankModel> bankModel = new List<BankModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT Id,BankName,AccountName,AccountNumber,Branch,SignaturePicture FROM Bank WHERE IsDeleted = 0 " +
                            "ORDER BY BankName ";
                bankModel = con.Query<BankModel>(query).ToList();
            }

            return bankModel;
        }

        public int InsertBank(BankModel bankModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                 result = commonRepository.GetValidateUnique("Bank", "BankName", bankModel.BankName, bankModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                int MaxId = commonRepository.GetMaxId("Bank");

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO Bank (Id,BankName," +
                            "AccountName,AccountNumber,Branch,SignaturePicture) " +
                            "VALUES (" + MaxId + ",@BankName," +
                            "@AccountName,@AccountNumber,@Branch,@SignaturePicture " +
                            "); SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, bankModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();

                    string output= commonRepository.SyncTableStatus("Bank"); 
                }
                else
                {
                    sqltrans.Rollback();
                }
            }

            return result;
        }

        public int UpdateBank(BankModel bankModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("Bank", "BankName", bankModel.BankName, bankModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE Bank SET BankName =@BankName," +
                            "AccountName = @AccountName, " +
                            "AccountNumber = @AccountNumber,"+
                            "Branch =@Branch,"+
                            "SignaturePicture =@SignaturePicture " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, bankModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                    string output = commonRepository.SyncTableStatus("Bank");
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
