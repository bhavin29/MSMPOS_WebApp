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
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IPaymentMethodRepository _IPaymentMethodReportsitory;

        public PaymentMethodService(IPaymentMethodRepository iAddondRepository)
        {
            _IPaymentMethodReportsitory = iAddondRepository;
        }
        public PaymentMethodModel GetAddonesById(int PaymentMethodId)
        {
            return _IPaymentMethodReportsitory.GetPaymentMethodList().Where(x => x.Id == PaymentMethodId).FirstOrDefault();
        }

        public List<PaymentMethodModel> GetPaymentMethodList()
        {

            return _IPaymentMethodReportsitory.GetPaymentMethodList();
        }

        public int InsertPaymentMethod(PaymentMethodModel PaymentMethodModel)
        {
            return _IPaymentMethodReportsitory.InsertPaymentMethod(PaymentMethodModel);
        }

        public int UpdatePaymentMethod(PaymentMethodModel PaymentMethodModel)
        {
            return _IPaymentMethodReportsitory.UpdatePaymentMethod(PaymentMethodModel);
        }

        public int DeletePaymentMethod(int PaymentMethodID)
        {
            return _IPaymentMethodReportsitory.DeletePaymentMethod(PaymentMethodID);
        }

    }
}
