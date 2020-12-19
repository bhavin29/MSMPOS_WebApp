using RocketPOS.Interface.Services;
using RocketPOS.Interface.Repository;
using RocketPOS.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RocketPOS.Services
{
    public class IngredientUnitService : IIngredientUnitService
    {

        private readonly IIngredientUnitRepository _IIngredientUnitRepositoy;

        public IngredientUnitService(IIngredientUnitRepository ingredientUnitRepositoy)
        {
            _IIngredientUnitRepositoy = ingredientUnitRepositoy;
        }

        public IngredientUnitModel GetIngredientUnitById(int ingredientUnitId)
        {
            return _IIngredientUnitRepositoy.GetIngredientUnitList().Where(x => x.Id == ingredientUnitId).FirstOrDefault();
        }

        public int InsertIngredientUnit(IngredientUnitModel ingredientUnitModel)
        {
            return _IIngredientUnitRepositoy.InsertIngredientUnit(ingredientUnitModel);
        }

        public int UpdateIngredientUnit(IngredientUnitModel ingredientUnitModel)
        {
            return _IIngredientUnitRepositoy.UpdateIngredientUnit(ingredientUnitModel);
        }

        public int DeleteIngredientUnit(int ingredientUnitId)
        {
            return _IIngredientUnitRepositoy.DeleteIngredientUnit(ingredientUnitId);
        }
     
        List<IngredientUnitModel> IIngredientUnitService.GetIngredientUnitList()
        {
            return _IIngredientUnitRepositoy.GetIngredientUnitList();
        }
    }
}
