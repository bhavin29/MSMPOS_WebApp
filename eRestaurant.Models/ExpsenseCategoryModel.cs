using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RocketPOS.Models
{
    public class ExpsenseCategoryModel
    {

        public int Id { get; set; }

        [DisplayName("Customer Name")]
        [Required]
        public string ExpenseCategory { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public int UserId { get; set; }


    }
}
