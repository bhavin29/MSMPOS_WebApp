using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
    public class OutletRegisterModel
    {
        public int Id { get; set; }
  
        [Required(ErrorMessage = "Select Outlet")]
        public int OutletId { get; set; }
       
        [DisplayName("Outlet")]
        public string OutletName { get; set; }

        public List<SelectListItem> OutletList { get; set; }

        [Required(ErrorMessage = "Select User")]
        public int UserId { get; set; }
   
        [DisplayName("User")]
        public string UserName { get; set; }

        public List<SelectListItem> UserList { get; set; }
 
        [DisplayName("Register Open")]
        [DataType(DataType.Date)]
        public DateTime OpenDate { get; set; }
       
        [DisplayName("Opening Balance")]
        [Required(ErrorMessage = "Enter Opening Balance")]
        public string OpeningBalance { get; set; }
 
        [DisplayName("No of Transaction")]
        public int TotalTransaction { get; set; }

        [DisplayName("Total Sales")]
        public decimal TotalAmount { get; set; }

        [DisplayName("Balance")]
        public decimal Balance { get; set; }
        
        [DisplayName("Register Close")]
        public DateTime CloseDateTime { get; set; }

        public int ApprovalUserId { get; set; }

        [DisplayName("Approval by")]
        public int ApprovalUserNAme { get; set; }

        [DisplayName("Approval At")]
        public DateTime ApprovalDateTime { get; set; }

        [DisplayName("Approval Amount")]
        public decimal ApprovalAmount { get; set; }

        [DisplayName("Notes")]
        public string ApprovalNotes { get; set; }
    }
}
