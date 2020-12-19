using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;
namespace RocketPOS.Models
{
   public class EmployeeAttendanceModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LogDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH\\:MM}", ApplyFormatInEditMode = true)]
        public DateTime InTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH\\:MM}", ApplyFormatInEditMode = true)]
        public DateTime OutTime { get; set; }
        public float TotalTimeCount { get; set; }
        public int UpdateStatus { get; set; }
        public string Notes { get; set; }


    }
}
