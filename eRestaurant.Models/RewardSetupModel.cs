using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RocketPOS.Models
{
    public class RewardSetupModel
    {
        public int Id { get; set; }

        [DisplayName("Offer Name")]
        [Required(ErrorMessage = "Enter Offer Name")]
        public string OfferName { get; set; }

        public decimal TransactionAmount { get; set; }
        public decimal RewardPoint { get; set; }
        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public int UserId { get; set; }

        public RewardSetupModel()
        {
            IsActive = true;
        }
    }
}
