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
    public class FoodMenuRepository : IFoodMenuRpository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public FoodMenuRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<FoodMenuModel> GetFoodMenuList()
        {
            List<FoodMenuModel> FoodMenuModel = new List<FoodMenuModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT  FM.Id, FoodCategoryId,FoodMenuCategoryName AS FoodCategoryName , FoodMenuName, FoodMenuCode, ColourCode, BigThumb, MediumThumb, SmallThumb," +
                            "SalesPrice, FM.Notes, IsVegItem, IsBeverages, FoodVat, Foodcess, OfferIsAvailable, "+
                            "FM.Position,  OutletId, FM.IsActive FROM FoodMenu FM INNER JOIN FoodMenuCategory FMC on FM.FoodCategoryId = FMC.Id WHERE FM.IsDeleted = 0 " +
                            "ORDER BY FoodMenuName ";
                FoodMenuModel = con.Query<FoodMenuModel>(query).ToList();
            }

            return FoodMenuModel;
        }

        public int InsertFoodMenu(FoodMenuModel foodMenuModel)
        {
            int result = 0;
            int detailsResult = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO FoodMenu "+
                    "(  FoodCategoryId, FoodMenuName, FoodMenuCode, ColourCode, BigThumb, MediumThumb, SmallThumb, SalesPrice, Notes, IsVegItem, IsBeverages,"+
                    " FoodVat, Foodcess, OfferIsAvailable,  Position,  OutletId, IsActive) " +
                    "Values "+
                    "(  @FoodCategoryId, @FoodMenuName, @FoodMenuCode, @ColourCode, @BigThumb, @MediumThumb, @SmallThumb,@SalesPrice, @Notes, " +
                    "@IsVegItem, @IsBeverages,@FoodVat,@Foodcess, @OfferIsAvailable,  @Position,  @OutletId, @IsActive)" +
                    " SELECT CAST(SCOPE_IDENTITY() as INT);";
                
                result = con.Execute(query, foodMenuModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    
                    foreach (var item in foodMenuModel.FoodMenuDetails)
                    {
                        var detailsQuery = "insert into FoodMenuIngredient (FoodMenuId, IngredientId , Consumption) values (" +
                            "" + result + "," +
                            "" + item.IngredientId + "," +
                            "" + item.Consumption + "); SELECT CAST(SCOPE_IDENTITY() as INT);";
                        detailsResult = con.Execute(detailsQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                    }
                }
                if (detailsResult > 0)
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

        public int UpdateFoodMenu(FoodMenuModel foodMenuModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE FoodMenu SET "+
                     "FoodCategoryId=@FoodCategoryId, FoodMenuName=@FoodMenuName, FoodMenuCode=@FoodMenuCode, ColourCode=@ColourCode,"+
                     "BigThumb=@BigThumb, MediumThumb=@MediumThumb, SmallThumb=@SmallThumb,SalesPrice=@SalesPrice, Notes=@Notes, "+
                     "IsVegItem=@IsVegItem, IsBeverages=@IsBeverages,FoodVat=@FoodVat, Foodcess=@Foodcess, OfferIsAvailable=@OfferIsAvailable,"+
                     "Position=@Position, OutletId=@OutletId, IsActive=@IsActive "+
                    "WHERE Id = @Id;";
                result = con.Execute(query, foodMenuModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int DeleteFoodMenu(int foodMenuId)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE FoodMenu SET IsDeleted = 1 WHERE Id = {foodMenuId};";
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
