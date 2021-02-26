using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Models
{
  public class LoginModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleTypeId { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string ClientName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Logo { get; set; }
        public string WebSite { get; set; }
        public string ReceiptPrefix { get; set; }
        public string OrderPrefix { get; set; }
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
        public int CurrencyId { get; set; }
        public string TimeZone { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        public string Footer1 { get; set; }
        public string Footer2 { get; set; }
        public string Footer3 { get; set; }
        public string Footer4 { get; set; }
        public string MainWindowSettings { get; set; }
        public string HeaderMarqueeText { get; set; }
        public string DeliveryList { get; set; }
        public string DiscountList { get; set; }
        public string Powerby { get; set; }
        public int TaxInclusive { get; set; }
        public bool IsItemOverright { get; set; }
        public string VATLabel { get; set; }
        public string PINLabel { get; set; }
        public string FromEmailAddress { get; set; }
        public string EmailDisplayName { get; set; }
        public string FromEmailPassword { get; set; }
        public string EmailSubject { get; set; }
    }
}
