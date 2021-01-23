using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IEmailService
    {
        void SendEmailToForFoodMenuPurchase(PurchaseModel purchaseModel,string email);
    }
}
