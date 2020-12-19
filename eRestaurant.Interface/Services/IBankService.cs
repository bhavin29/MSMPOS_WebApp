using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IBankService
    {
        List<BankModel> GetBankList();

        BankModel GetBankById(int bankId);
        int InsertBank(BankModel bankModel);

        int UpdateBank(BankModel bankModel);

        int DeleteBank(int bankId);
    }
}
