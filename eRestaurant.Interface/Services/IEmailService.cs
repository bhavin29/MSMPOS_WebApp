using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IEmailService
    {
        void SendEmailToForFoodMenuPurchase(int purchaseId, ClientModel clientModel);
    }
}
