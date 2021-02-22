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
    public class SupplierRepository : ISupplierRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public SupplierRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<SupplierModel> GetSupplierList()
        {
            List<SupplierModel> supplierModel = new List<SupplierModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT Id, SupplierName,TaxType,SupplierAddress1, SupplierAddress2,SupplierPhone,SupplierEmail,SupplierPicture, IsActive,Vat As VATNumber,Pin As PINNumber from Supplier WHERE IsDeleted = 0 " +
                            "ORDER BY SupplierName ";
                supplierModel = con.Query<SupplierModel>(query).ToList();
            }

            return supplierModel;
        }

        public int InsertSupplier(SupplierModel supplierModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("Supplier", "SupplierName", supplierModel.SupplierName, supplierModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO Supplier (SupplierName,TaxType,SupplierAddress1, SupplierAddress2,SupplierPhone,SupplierEmail,SupplierPicture, IsActive,Vat,Pin)" +
                            "VALUES " +
                            "(@SupplierName,@TaxType,@SupplierAddress1, @SupplierAddress2,@SupplierPhone,@SupplierEmail,@SupplierPicture, @IsActive,@VATNumber,@PINNumber);" +
                            " SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, supplierModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int UpdateSupplier(SupplierModel supplierModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("Supplier", "SupplierName", supplierModel.SupplierName, supplierModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE Supplier SET  SupplierName=@SupplierName,TaxType=@TaxType,"+
                            "SupplierAddress1=@SupplierAddress1, SupplierAddress2=@SupplierAddress2,SupplierPhone=@SupplierPhone," +
                            "SupplierEmail =@SupplierEmail,SupplierPicture=@SupplierPicture, IsActive=@IsActive, " +
                            "Vat=@VATNumber,Pin=@PINNumber " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, supplierModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int DeleteSupplier(int supplierId)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE Supplier SET IsDeleted = 1 WHERE Id = {supplierId};";
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
