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
        public void SendEmailToForFoodMenuPurchase(PurchaseModel purchaseModel,string email)
        {
            string messageBody = "<font>The following are the orders: </font><br><br>";
            string htmlTableStart = "<table style=\"border-collapse:collapse; text-align:center;\" >";
            string htmlTableEnd = "</table>";
            string htmlHeaderRowStart = "<tr style=\"background-color:#6FA1D2; color:#ffffff;\">";
            string htmlHeaderRowEnd = "</tr>";
            string htmlTrStart = "<tr style=\"color:#555555;\">";
            string htmlTrEnd = "</tr>";
            string htmlTdStart = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">";
            string htmlTdEnd = "</td>";

            messageBody += htmlTableStart;
            messageBody += htmlHeaderRowStart;
            messageBody += htmlTdStart + "Food Menu" + htmlTdEnd;
            messageBody += htmlTdStart + "Price" + htmlTdEnd;
            messageBody += htmlTdStart + "Qty" + htmlTdEnd;
            messageBody += htmlTdStart + "Total" + htmlTdEnd;
            messageBody += htmlHeaderRowEnd;

            foreach (var foodItem in purchaseModel.PurchaseDetails)
            {
                messageBody = messageBody + htmlTrStart;
                messageBody = messageBody + htmlTdStart + foodItem.FoodMenuName + htmlTdEnd;
                messageBody = messageBody + htmlTdStart + foodItem.UnitPrice + htmlTdEnd; 
                messageBody = messageBody + htmlTdStart + foodItem.Quantity + htmlTdEnd; 
                messageBody = messageBody + htmlTdStart + foodItem.Total + htmlTdEnd; 
                messageBody = messageBody + htmlTrEnd;
            }
            
            messageBody = messageBody + htmlTableEnd;
            SendEmail.Email(messageBody,email);
        }
    }
}
