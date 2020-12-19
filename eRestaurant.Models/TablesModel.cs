using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class TablesModel
    {
        public int Id { get; set; }

        public int OutletId { get; set; }
        public string OutletName { get; set; }
        public List<SelectListItem> OutletList { get; set; }
        public string TableName { get; set; }
        public int PersonCapacity { get; set; }
        public string TableIcon { get; set; }
        public int Status { get; set; }
    }
}
