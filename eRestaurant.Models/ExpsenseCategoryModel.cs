﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RocketPOS.Models
{
    public class ExpsenseCategoryModel
    {

        public int Id { get; set; }

        [DisplayName("Expense Category")]
        [Required(ErrorMessage = "Enter Expense Category")]
        public string ExpenseCategory { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public int UserId { get; set; }

        public ExpsenseCategoryModel()
        {
            IsActive = true;
        }
    }
}
