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
using RocketPOS.Framework;

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
                var query = " SELECT  FM.Id, FoodCategoryId,FoodMenuCategoryName AS FoodCategoryName , FoodMenuType, FoodMenuName, FoodMenuCode, ColourCode, BigThumb, MediumThumb, SmallThumb," +
                            " SalesPrice, PurchasePrice,FM.Notes, IsVegItem, IsBeverages, FoodVat, Foodcess, OfferIsAvailable, " +
                            " FM.Position,  OutletId, FM.IsActive, U.UnitName,T.TaxName  " +
                            " FROM FoodMenu FM INNER JOIN FoodMenuCategory FMC on FM.FoodCategoryId = FMC.Id " +
                            " Left join Units U on U.Id = FM.UnitsId " +
                            " Left join Tax T On T.Id = FM.FoodVatTaxId " + 
                            " WHERE FM.IsDeleted = 0 " +
                            " ORDER BY FoodMenuCode,FoodMenuName ";
                FoodMenuModel = con.Query<FoodMenuModel>(query).ToList();
            }

            return FoodMenuModel;
        }

        public List<FoodMenuModel> GetFoodMenuById(long foodMenuId)
        {
            List<FoodMenuModel> foodManuModelList = new List<FoodMenuModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT  FM.Id, FM.UnitsId,FM.FoodVatTaxId, FoodMenuType,FoodCategoryId,FoodMenuCategoryName AS FoodCategoryName,FoodMenuName, FoodMenuCode, ColourCode, BigThumb, MediumThumb, SmallThumb," +
                            "SalesPrice, PurchasePrice,FM.Notes, IsVegItem, IsBeverages, FoodVat, Foodcess, OfferIsAvailable, " +
                            "FM.Position,  OutletId, FM.IsActive FROM FoodMenu FM INNER JOIN FoodMenuCategory FMC on FM.FoodCategoryId = FMC.Id WHERE FM.IsDeleted = 0 and " +
                            " FM.Id = " + foodMenuId +
                            " ORDER BY FoodMenuName;";

                foodManuModelList = con.Query<FoodMenuModel>(query).AsList();
            }
            return foodManuModelList;
        }

        public List<FoodManuDetailsModel> GetFoodMenuDetails(long foodMenuId)
        {
            List<FoodManuDetailsModel> foodMenuDetails = new List<FoodManuDetailsModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {

                var query = "select FoodMenuId,IngredientId,i.ingredientName,Consumption from FoodmenuIngredient as a inner join foodmenu b on " +
                    "a.foodMenuId = b.id inner join ingredient as i on a.IngredientId = i.id where b.id = " + foodMenuId + ";";

                foodMenuDetails = con.Query<FoodManuDetailsModel>(query).AsList();
            }

            return foodMenuDetails;
        }

        public int InsertFoodMenu(FoodMenuModel foodMenuModel)
        {
            int result = 0;
            int detailsResult = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                int MaxId = commonRepository.GetMaxId("FoodMenu");

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO FoodMenu " +
                    "(  Id,FoodCategoryId, FoodMenuName, FoodMenuType,FoodMenuCode, PurchasePrice,SalesPrice, Notes, UnitsId,FoodVatTaxId," +
                    " Position,  IsActive) " +
                    "Values " +
                    "(" + MaxId + ",  @FoodCategoryId, upper(@FoodMenuName),@FoodMenuType, @FoodMenuCode, @PurchasePrice,@SalesPrice, @Notes,@UnitsId,@FoodVatTaxId," +
                  "@Position,  @IsActive);" +
                    " SELECT CAST(SCOPE_IDENTITY() as INT);";

                result = con.Execute(query, foodMenuModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();

                    //CREATE ENTRY INTO INVETORY AS STOCK 0.00
                    query = " INSERT INTO INVENTORY (STOREID,FOODMENUID,STOCKQTY,USERIDINSERTED,ISDELETED)" +
                            " Select S.ID as StoreId,FM.Id,0,1,0 from foodmenu FM CROSS JOIN STORE S " +
                            " WHERE FM.ID =" + MaxId;

                    result = con.Execute(query, foodMenuModel, sqltrans, 0, System.Data.CommandType.Text);

                    //CREATE ENTRY INTO FOODMENURATE
                    query = " INSERT INTO FOODMENURATE(Id, OutletId, FoodMenuId, SalesPrice, FoodVatTaxId, IsActive) " +
                            " Select(select max(Id) from foodmenurate) + ROW_NUMBER() OVER(ORDER BY fm.id desc) AS Row# " +
                            " , O.Id,FM.Id,FM.SalesPrice,FM.FoodVatTaxId,1 from FoodMenu FM Cross join Outlet O " +
                            " Where FM.Id =" + MaxId;

                    result = con.Execute(query, foodMenuModel, sqltrans, 0, System.Data.CommandType.Text);


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
            int detailsResult = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE FoodMenu SET " +
                     "FoodCategoryId=@FoodCategoryId, " +
                     "FoodMenuName=@FoodMenuName, " +
                     "FoodMenuType=@FoodMenuType, " +
                     "FoodMenuCode=@FoodMenuCode, " +
                     "PurchasePrice=@PurchasePrice, " +
                     " UnitsId = @UnitsId," +
                     " FoodVatTaxId =@FoodVatTaxId ," +
                     "ColourCode=@ColourCode," +
                     "BigThumb=@BigThumb, " +
                     "MediumThumb=@MediumThumb, " +
                     "SmallThumb=@SmallThumb, " +
                     "SalesPrice=@SalesPrice, " +
                     "Notes=@Notes, " +
                     "IsVegItem=@IsVegItem, " +
                     "IsBeverages=@IsBeverages,FoodVat=@FoodVat, " +
                     "Foodcess=@Foodcess, " +
                     "OfferIsAvailable=@OfferIsAvailable," +
                     "Position=@Position, " +
                     "OutletId=@OutletId, " +
                     "IsActive=@IsActive " +
                     ",[UserIdUpdated] = " + LoginInfo.Userid + " " +
                     ",[DateUpdated]  = GetUtcDate() WHERE Id = @Id;";
                result = con.Execute(query, foodMenuModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    int detailResult = 0;
                    if (foodMenuModel.DeletedId != null)
                    {
                        foreach (var item in foodMenuModel.DeletedId)
                        {
                            var deleteQuery = $"update FoodMenuIngredient set IsDeleted = 1, UserIdDeleted = " + LoginInfo.Userid + ", DateDeleted = GetutcDate() where id = " + item + ";";
                            result = con.Execute(deleteQuery, null, sqltrans, 0, System.Data.CommandType.Text);
                        }
                    }
                    //foreach (var item in foodMenuModel.FoodMenuDetails)
                    //{
                    //    var queryDetails = string.Empty;
                    //    if (item.FoodMenuId > 0)
                    //    {
                    //        queryDetails = "Update [dbo].[FoodMenuIngredient] set " +
                    //                             " [FoodMenuId]  = " + item.FoodMenuId + "," +
                    //                             " [IngredientId]   = " + item.IngredientId + "," +
                    //                             " [Consumption]        =  " + item.Consumption + "," +
                    //                             " [UserIdUpdated] = " + LoginInfo.Userid + "," +
                    //                             " [DateUpdated] = GetUTCDate() " +
                    //                             " where id = " + item.FoodMenuId + ";";
                    //    }
                    //    else
                    //    {
                    //        var detailsQuery = "insert into FoodMenuIngredient (FoodMenuId, IngredientId , Consumption,[UserIdUpdated] ) values (" +
                    //       "" + foodMenuModel.Id + "," +
                    //       "" + item.IngredientId + "," +
                    //       "" + item.Consumption + "," +
                    //        "" + LoginInfo.Userid + "); ";
                    //    }
                    //    detailResult = con.Execute(queryDetails, null, sqltrans, 0, System.Data.CommandType.Text);
                    //}

                    if (result > 0)
                    {
                        sqltrans.Commit();
                        //  query = $"UPDATE FoodMenuRate SET FoodVatTaxId = " + foodMenuModel.FoodVatTaxId + " WHERE FoodmenuId = {foodMenuId};";
                        //  result = con.Execute(query, null, sqltrans, 0, System.Data.CommandType.Text);

                    }
                    else
                    {
                        sqltrans.Rollback();
                    }
                }
                return result;
            }
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

                    query = $"UPDATE FoodMenuRate SET IsDeleted = 1 WHERE FoodmenuId = {foodMenuId};";
                    result = con.Execute(query, null, sqltrans, 0, System.Data.CommandType.Text);
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
