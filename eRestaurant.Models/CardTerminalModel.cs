using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class CardTerminalModel
    {
        public int Id { get; set; }
        public int OutletId { get; set; }
        public string Outlet { get; set; }
        public List<SelectListItem> OutletList { get; set; }


        [DisplayName("Card Terminal Name")]
        [Required(ErrorMessage = "Enter Card Terminal Name")]
        public string CardTerminalName { get; set; }

        public bool IsActive { get; set; }

        public CardTerminalModel()
        {
            IsActive = true;
        }
    }
}
