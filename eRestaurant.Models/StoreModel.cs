using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RocketPOS.Models
{
    public class StoreModel
    {
        public int Id { get; set; }

        [DisplayName("Store Name")]
        [Required]
        public string StoreName { get; set; }

        public bool IsMainStore { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public bool IsLock { get; set; }
        public int UserId { get; set; }
    }
}
