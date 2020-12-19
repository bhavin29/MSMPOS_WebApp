using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IPaymentMethodService
    {
        PaymentMethodModel GetAddonesById(int PaymentMethodId);
        List<PaymentMethodModel> GetPaymentMethodList();

        int InsertPaymentMethod(PaymentMethodModel PaymentMethodModel);

        int UpdatePaymentMethod(PaymentMethodModel PaymentMethodModel);

        int DeletePaymentMethod(int PaymentMethodID);
    }
}
