using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Models
{
    public class StoreModel
    {
        public int Id { get; set; }
        public string StoreName { get; set; }

        public bool IsMainStore { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public bool IsLock { get; set; }
        public int UserId { get; set; }
    }
}
