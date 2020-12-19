using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;


namespace RocketPOS.Interface.Repository
{
    public interface IBankRepository
    {

        List<BankModel> GetBankList();

        int InsertBank(BankModel bankModel);

        int UpdateBank(BankModel bankModel);

        int DeleteBank(int bankId);
    }
}
