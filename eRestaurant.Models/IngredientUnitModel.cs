﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RocketPOS.Models
{
    public class IngredientUnitModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter Unit name")]
        [DisplayName("Unit")]
        public string IngredientUnitName { get; set; }

        [Required(ErrorMessage = "Enter Unit short name")]
        [DisplayName("UnitShortName")]
        public string UnitShortName { get; set; }

        [Required(ErrorMessage = "Select unit precision")]
        [DisplayName("UnitPrecision")]
        public int UnitPrecision { get; set; }
        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public int UserId { get; set; }

        public IngredientUnitModel()
        {
            IsActive = true;
        }
    }
}
