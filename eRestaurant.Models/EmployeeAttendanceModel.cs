using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel;
using System.Web.Mvc;
namespace RocketPOS.Models
{
   public class EmployeeAttendanceModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Log Date")]
        public DateTime LogDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH\\:MM}", ApplyFormatInEditMode = true)]
        [DisplayName("IN Time")]
        public DateTime InTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH\\:MM}", ApplyFormatInEditMode = true)]
        [DisplayName("OUT Time")]
        public DateTime OutTime { get; set; }
        [DisplayName("TotalWork Hours")]
        public float TotalTimeCount { get; set; }
        [DisplayName("Update Status")]

        public int UpdateStatus { get; set; }
        public string Notes { get; set; }


    }
}
