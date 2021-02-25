using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RocketPOS.Models
{
    public class RawMaterialModel
    {
        public int Id { get; set; }

        [DisplayName("Raw Material Name")]
        [Required(ErrorMessage = "Enter Raw Material Name")]
        public string RawMaterialName { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public int UserId { get; set; }

        public RawMaterialModel()
        {
            IsActive = true;
        }
    }
}
