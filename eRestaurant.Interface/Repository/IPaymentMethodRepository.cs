using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;
namespace RocketPOS.Interface.Repository
{
    public interface IPaymentMethodRepository
    {
        List<PaymentMethodModel> GetPaymentMethodList();

        int InsertPaymentMethod(PaymentMethodModel PaymentMethodModel);

        int UpdatePaymentMethod(PaymentMethodModel PaymentMethodModel);

        int DeletePaymentMethod(int PaymentMethodID);

    }
}
