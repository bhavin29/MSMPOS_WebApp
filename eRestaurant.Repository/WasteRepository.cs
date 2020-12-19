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
    public class WasteRepository : IWasteRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;
        public WasteRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<WasteModel> GetWasteList()
        {
            List<WasteModel> WasteModel = new List<WasteModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
                {
                    var query = "select ing.id,ing.WasteName as WasteName, category.WasteCategoryName as Category," +
                                 "unit.WasteUnitName as Unit, ing.IsActive , ing.WasteCategoryId as CategoryId,ing.WasteUnitId as UnitId" +
                                 ",ing.PurchasePrice, ing.SalesPrice, ing.AlterQty,ing.Code" +
                                 " from Waste as Ing inner join WasteCategory as category " +
                                 "on ing.WasteCategoryId = category.id and category.IsDeleted = 0 inner join WasteUnit as unit " +
                                 "on ing.WasteUnitId = unit.Id and unit.IsDeleted = 0 where ing.IsDeleted = 0";
                    WasteModel = con.Query<WasteModel>(query).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return WasteModel;
        }

        public int InsertWaste(WasteModel WasteModel)
        {
            int result = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
                {
                    con.Open();
                    SqlTransaction sqltrans = con.BeginTransaction();
                    var query = "INSERT INTO Waste(WasteName," +
                        "Code, " +
                        "WasteCategoryId," +
                        "WasteUnitId," +
                        "PurchasePrice," +
                        "SalesPrice," +
                        "AlterQty," +
                        "IsActive) " +
                        "VALUES(@WasteName," +
                        " @Code," +
                        " @CategoryId," +
                        "@UnitId," +
                        "@PurchasePrice," +
                       "@SalesPrice," +
                       "@AlterQty," + "@IsActive" + " ); SELECT CAST(SCOPE_IDENTITY() as INT); ";
                    result = con.Execute(query, WasteModel, sqltrans, 0, System.Data.CommandType.Text);

                    if (result > 0)
                    {
                        sqltrans.Commit();
                    }
                    else
                    {
                        sqltrans.Rollback();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int UpdateWaste(WasteModel WasteModel)
        {
            int result = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
                {
                    con.Open();
                    SqlTransaction sqltrans = con.BeginTransaction();
                    var query = "Update Waste set WasteName = @WasteName," +
                         "Code = @Code ," +
                         "WasteCategoryId = @CategoryId," +
                         "WasteUnitId = @UnitId," +
                         "PurchasePrice = @PurchasePrice," +
                         "SalesPrice = @SalesPrice," +
                         "AlterQty = @AlterQty," +
                         "IsActive = @IsActive WHERE Id = @Id ";
                    result = con.Execute(query, WasteModel, sqltrans, 0, System.Data.CommandType.Text); ;
                    if (result > 0)
                    {
                        sqltrans.Commit();
                    }
                    else
                    {
                        sqltrans.Rollback();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int DeleteWaste(int WasteId)
        {
            int result = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
                {
                    con.Open();
                    SqlTransaction sqltrans = con.BeginTransaction();
                    var query = $"update Waste set IsDeleted = 1 where id = {WasteId}";
                    result = con.Execute(query, null, sqltrans, 0, System.Data.CommandType.Text);
                    if (result > 0)
                    {
                        sqltrans.Commit();
                    }
                    else
                    { sqltrans.Rollback(); }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

     
    }
}
