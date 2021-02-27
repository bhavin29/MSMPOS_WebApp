using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class GlobalStatusModel
    {
        public int Id { get; set; }
        
        [DisplayName("Module")]
        [Required(ErrorMessage = "Select Module Name")]
        public string ModuleName { get; set; }
        public List<SelectListItem> ModuleList { get; set; }
        [DisplayName("Status Name")]
        [Required(ErrorMessage = "Enter Status Name")]
        public string StatusName { get; set; }
        public string StatusCode { get; set; }
    }
}
