using RocketPOS.Framework;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmailToForFoodMenuPurchase(int purchaseId, ClientModel clientModel)
        {
            string messageBody = "<html><body><p>Hello</p><p>You have a purchase order for your approval.</p>";
            messageBody += "<p>Kindly review - <a href=" + clientModel.WebAppUrl + "/"+purchaseId+">click here</a></p>";
            messageBody += "<p>Thanks</p><p>RocketPOS Team</p><p>PS: This is auto-generated mail from the system. Please do not reply to it.</p></body></html>";
            SendEmail.Email(messageBody, clientModel.PurchaseApprovalEmail);
        }
    }
}
