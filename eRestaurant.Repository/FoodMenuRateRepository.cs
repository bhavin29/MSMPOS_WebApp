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
    public class FoodMenuRateRepository : IFoodMenuRateRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public FoodMenuRateRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }
        public List<FoodMenuRate> GetFoodMenuRateList(int foodCategoryId, int outletId)
        {
            List<FoodMenuRate> foodMenuRate = new List<FoodMenuRate>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "select FMR.Id,FMR.OutletId,O.OutletName,F.FoodMenuName,FMR.FoodMenuId,FMR.SalesPrice,FMR.IsActive from FoodMenuRate FMR " +
                            "Inner Join FoodMenu F On F.Id = FoodMenuId Left Join Outlet O On O.Id = FMR.OutletId  Where F.ISdeleted=0 ";

                if (foodCategoryId != 0)
                {
                    query = query + " and F.FoodCategoryId = " + foodCategoryId +" ";
                }

                if (outletId != 0)
                {
                    query = query + " and FMR.outletId=" + outletId + " ";
                }

                foodMenuRate = con.Query<FoodMenuRate>(query).ToList();
            }
            return foodMenuRate;
        }

        public int UpdateFoodMenuRateList(List<FoodMenuRate> foodMenuRates)
        {
            int result = 0;
            List<FoodMenuRate> foodMenuRate = new List<FoodMenuRate>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                foreach (var item in foodMenuRates)
                {
                    var query = $"Update FoodMenuRate set SalesPrice=@SalesPrice,IsActive=@IsActive,UserIdUpdated=" + LoginInfo.Userid + ", DateUpdated=GetutcDate() where id=@Id";
                    result = con.Execute(query, item, sqltrans, 0, System.Data.CommandType.Text);
                }
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
