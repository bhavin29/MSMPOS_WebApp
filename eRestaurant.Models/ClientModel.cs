using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Models
{
    public class ClientModel
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string StoreName { get; set; }
        public string Adress1 { get; set; }
        public string Adress2 { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Logo { get; set; }
        public string FavIcon { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime Closeime { get; set; }
        public int CurrencyId { get; set; }
        public string CurIcon { get; set; }
        public string MinpreparreTime { get; set; }
        public string TimeZone { get; set; }
        public int RowPerpage { get; set; }
        public int MaxPagePerSheet { get; set; }
        public string SyncShortDelay { get; set; }
        public string SyncLongDelay { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        public int UserId { get; set; }
        public string WebAppUrl { get; set; }
        public string PurchaseApprovalEmail { get; set; }
    }
}
