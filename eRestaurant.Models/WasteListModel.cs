using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Models
{
    public class WasteListModel
    {
        public int Id { get; set; }
        public int WasteId { get; set; }
        public int IngredientId { get; set; }
        public decimal Qty { get; set; }
        public decimal LossAmount { get; set; }

    }
    }
