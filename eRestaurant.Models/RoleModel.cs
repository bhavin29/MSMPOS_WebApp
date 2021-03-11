using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RocketPOS.Models
{
    public class RoleModel
    {
        public int Id { get; set; }
        [DisplayName("Web Role Name")]
        [Required(ErrorMessage = "Enter Web Role Name")]
        public string WebRoleName { get; set; }
        public bool IsActive { get; set; }
        public RoleModel()
        {
            IsActive = true;
        }
    }
}
