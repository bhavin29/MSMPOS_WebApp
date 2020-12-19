using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace RocketPOS.Models
{
   public class UserModel
    {
        public int Id { get; set; }
        public int EmployeeId  { get; set; }

        public string EmployeeName { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }
        public string OutletId { get; set; }

        public string OutletName { get; set; }
        public List<SelectListItem> OutletList { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string ThumbToken { get; set; }
        public int RollTypeId { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime LastLogout { get; set; }
        public string IPAdress { get; set; }
        public string Counter { get; set; }
        public bool IsActive { get; set; }
       int UserID { get; set; }
    }
}
