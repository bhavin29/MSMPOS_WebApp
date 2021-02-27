using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RocketPOS.Models
{
    public class SectionModel
    {
        public int Id { get; set; }

        [DisplayName("Section Name")]
        [Required(ErrorMessage = "Enter Section Name")]
        public string SectionName { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public SectionModel()
        {
            IsActive = true;
        }
    }
}
