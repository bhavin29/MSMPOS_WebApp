using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Models
{
    public class ExpsenseCategoryModel
    {

        public int Id { get; set; }
        public string ExpenseCategory { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public int UserId { get; set; }


    }
}
