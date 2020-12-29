using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RocketPOS.Framework;

namespace RocketPOS.Models
{
   public class UserModel
    {
        public int Id { get; set; }
        public int EmployeeId  { get; set; }

        [DisplayName("Employee")]
        [Required(ErrorMessage = "Select Employee Name")]
        public string EmployeeName { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }

        [DisplayName("Outlet")]
        [Required(ErrorMessage = "Select Outlet")]
        public string OutletId { get; set; }

        [DisplayName("Outlet")]
        public string OutletName { get; set; }
        public List<SelectListItem> OutletList { get; set; }

        [DisplayName("Username")]
        [Required(ErrorMessage = "Enter Username")]
        public string Username { get; set; }
        public string Password { get; set; }

        public string ThumbToken { get; set; }

        [EnumDataType(typeof(RoleType))]

        [DisplayName("Role")]
        [Required(ErrorMessage = "Select Role")]
        public RoleType? RoleTypeId { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime LastLogout { get; set; }
        public string IPAdress { get; set; }
        public string Counter { get; set; }
        public bool IsActive { get; set; }
       int UserID { get; set; }


        public UserModel()
        {
            IsActive = true;
        }
    }
}
