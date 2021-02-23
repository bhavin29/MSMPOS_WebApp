using RocketPOS.Interface.Services;
using RocketPOS.Interface.Repository;
using RocketPOS.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System;

namespace RocketPOS.Services
{
    public class WasteService : IWasteService
    {
        private readonly IWasteRepository _iWasteRepository;
        // private IWasteRepository _iWasteRepository;

        public WasteService(IWasteRepository iWasteRepository)
        {
            _iWasteRepository = iWasteRepository;
        }

        public List<WasteListModel> GetWasteList(int foodMenuId, int ingredientId)
        {
            return _iWasteRepository.GetWasteList(foodMenuId, ingredientId);
        }
        public int InsertWaste(WasteModel wasteModel)
        {
            return _iWasteRepository.InsertWaste(wasteModel);
        }
        public int UpdateWaste(WasteModel wasteModel)
        {
            return _iWasteRepository.UpdateWaste(wasteModel);
        }
        public int DeleteWaste(long wasteId)
        {
            return _iWasteRepository.DeleteWaste(wasteId);
        }

        public int DeleteWasteDetails(long wasteId, long foodManuId, long ingredientId)
        {
            return _iWasteRepository.DeleteWasteDetails(wasteId, foodManuId, ingredientId);
        }

        public long ReferenceNumber()
        {
            return _iWasteRepository.ReferenceNumber();
        }

        public List<WasteDetailModel> GetWasteDetails(long wasteId)
        {
            throw new System.NotImplementedException();
        }

        public WasteModel GetWasteById(long wasteId)
        {
            List<WasteModel> wasteModel = new List<WasteModel>();
            WasteModel model = new WasteModel();

            model = _iWasteRepository.GetWasteById(wasteId).ToList().SingleOrDefault();
            if (model != null)
            {
                model.WasteDetail = _iWasteRepository.GetWasteDetails(wasteId);
            }
            return model;
        }
        public List<SelectListItem> FoodMenuListForLostAmount()
        {
            List<SelectListItem> foodManuForLostAmount = new List<SelectListItem>();

            foodManuForLostAmount.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            List<DropDownModel> foodManuForLostAmountResult = _iWasteRepository.FoodMenuListForLostAmount();
            if (foodManuForLostAmountResult != null && foodManuForLostAmountResult.Count > 0)
            {
                foreach (var item in foodManuForLostAmountResult)
                {
                    foodManuForLostAmount.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
            }

            return foodManuForLostAmount;
        }

        public List<SelectListItem> IngredientListForLostAmount()
        {
            List<SelectListItem> ingredientForLostAmount = new List<SelectListItem>();

            ingredientForLostAmount.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            List<DropDownModel> ingredientForLostAmountResult = _iWasteRepository.IngredientListForLostAmount();
            if (ingredientForLostAmountResult != null && ingredientForLostAmountResult.Count > 0)
            {
                foreach (var item in ingredientForLostAmountResult)
                {
                    ingredientForLostAmount.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
            }

            return ingredientForLostAmount;


        }

        public decimal GetFoodMenuPurchasePrice(int id)
        {
            return _iWasteRepository.GetFoodMenuPurchasePrice(id);
        }

        public decimal GetIngredientPurchasePrice(int id)
        {
            return _iWasteRepository.GetIngredientPurchasePrice(id);
        }
    }
}
