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
            int result = 0, storeCount = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("Outlet", "OutletName", outletModel.OutletName, outletModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                int MaxId = commonRepository.GetMaxId("Outlet");

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();

                var queryStoreCount = "select count(storeId) from Outlet where IsActive=1 And IsDeleted=0 And storeId=" + outletModel.StoreId;
                storeCount = con.QueryFirstOrDefault<int>(queryStoreCount, null, sqltrans);

                if (storeCount == 0)
                {
                    var query = "INSERT INTO Outlet" +
                            "(Id,StoreId, OutletName, OutletAddress1, OutletAddress2, OutletPhone, OutletEmail," +
                            " InvoiceHeader, InvoiceFooter, IsCollectTax, IsPreorPostPayment, IsActive, IsLock)" +
                           "VALUES " +
                           "(" + MaxId + ", @StoreId, @OutletName, @OutletAddress1, @OutletAddress2, @OutletPhone, @OutletEmail, " +
                           "@InvoiceHeader, @InvoiceFooter, @IsCollectTax, @IsPreorPostPayment, @IsActive, @IsLock); " +
                           " SELECT CAST(SCOPE_IDENTITY() as INT);";
                    result = con.Execute(query, outletModel, sqltrans, 0, System.Data.CommandType.Text);

                    if (result > 0)
                    {
                        sqltrans.Commit();

                        //CREATE ENTRY INTO FOODMENURATE
                        query = " INSERT INTO FOODMENURATE(Id, OutletId, FoodMenuId, SalesPrice, FoodVatTaxId, IsActive)  " +
                                " Select(select max(Id) from foodmenurate) + ROW_NUMBER() OVER(ORDER BY fm.id desc) AS Row# , " +
                                 MaxId + ", FM.Id,FM.SalesPrice,FM.FoodVatTaxId,1 FROM FoodMenu FM WHERE isdeleted = 0 ";

                        result = con.Execute(query, outletModel, sqltrans, 0, System.Data.CommandType.Text);
                    }
                    else
                    {
                        sqltrans.Rollback();
                    }
                }
                else
                {
                    result = -1;
                }
            }

            return result;
        }

        public int UpdateOutlet(OutletModel outletModel)
        {
            int result = 0, storeCount = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("Outlet", "OutletName", outletModel.OutletName, outletModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();

                if (outletModel.OriginalStoreId != outletModel.StoreId)
                {
                    var queryStoreCount = "select count(storeId) from Outlet where IsActive=1 And IsDeleted=0 And storeId=" + outletModel.StoreId;
                    storeCount = con.QueryFirstOrDefault<int>(queryStoreCount, null, sqltrans);
                }

                if (storeCount == 0)
                {
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
                else
                {
                    result = -1;
                }
            }
            return result;
        }

        public int DeleteOutlet(int addonsId)
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
