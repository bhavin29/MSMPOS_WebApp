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
    public class BankService :IBankService
    {
        private readonly IBankRepository _ibankRepository;

        public BankService(IBankRepository iBankRepository)
        {
            _ibankRepository = iBankRepository;
        }

        public int DeleteBank(int bankId)
        {
            return _ibankRepository.DeleteBank(bankId);
        }

        public BankModel GetBankById(int bankId)
        {
            return _ibankRepository.GetBankList().Where(x => x.Id == bankId).FirstOrDefault();
        }

        public List<BankModel> GetBankList()
        {
            return _ibankRepository.GetBankList();
        }

        public int InsertBank(BankModel bankModel)
        {
            return _ibankRepository.InsertBank(bankModel);
        }

        public int UpdateBank(BankModel bankModel)
        {
            return _ibankRepository.UpdateBank(bankModel);
        }
    }
}
