using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RocketPOS.Framework;

namespace RocketPOS.Models
{
    public class TablesModel
    {
        public int Id { get; set; }

        public int OutletId { get; set; }

        [DisplayName("Outlet")]
        [Required(ErrorMessage = "Select Outlet")]
        public string OutletName { get; set; }
        public List<SelectListItem> OutletList { get; set; }

        [DisplayName("Table")]
        [Required(ErrorMessage = "Enter Table Name")]
        public string TableName { get; set; }
        [DisplayName("Person Capacity")]
        [Required] 
        public int PersonCapacity { get; set; }
        public string TableIcon { get; set; }

        [EnumDataType(typeof(TableStatus))]
        public TableStatus? Status { get; set; }
    }
}
