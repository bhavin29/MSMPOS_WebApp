using RocketPOS.Interface.Repository;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RocketPOS.Services
{
    public class TaxService: ITaxService
    {
        private readonly ITaxRepository _iTaxRepository;

        public TaxService(ITaxRepository iTaxRepository)
        {
            _iTaxRepository = iTaxRepository;
        }

        public int DeleteTax(int id)
        {
            return _iTaxRepository.DeleteTax(id);
        }

        public TaxModel GetTaxById(int id)
        {
            return _iTaxRepository.GetTaxList().Where(x => x.Id == id).FirstOrDefault();
        }

        public List<TaxModel> GetTaxList()
        {
            return _iTaxRepository.GetTaxList();
        }

        public int InsertTax(TaxModel taxModel)
        {
            return _iTaxRepository.InsertTax(taxModel);
        }

        public int UpdateTax(TaxModel taxModel)
        {
            return _iTaxRepository.UpdateTax(taxModel);
        }
    }
}
