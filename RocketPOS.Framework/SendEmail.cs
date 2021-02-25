using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace RocketPOS.Framework
{
    public static class SendEmail
    {
        public static void Email(string htmlString, string email)
        {
            try
            {
                var fromAddress = new MailAddress(LoginInfo.FromEmailAddress, LoginInfo.EmailDisplayName);
                var toAddress = new MailAddress(email, "");
                string fromPassword = LoginInfo.FromEmailPassword;
                string subject = LoginInfo.EmailSubject;
                string body = htmlString;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 20000
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml=true
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
