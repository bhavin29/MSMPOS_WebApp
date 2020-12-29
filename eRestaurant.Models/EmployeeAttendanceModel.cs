using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel;
using System.Web.Mvc;
using RocketPOS.Framework;

namespace RocketPOS.Models
{
    public class EmployeeAttendanceModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Select  Employee")]
        public int EmployeeId { get; set; }
        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }

        [DisplayName("Log Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Select  Log Date")]
        public DateTime LogDate { get; set; }
      
        [DisplayName("IN Time")]
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}")]
        [Required(ErrorMessage = "Select  IN Time")]
        public TimeSpan InTime { get; set; }

        [DisplayName("OUT Time")]
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}")]
        public TimeSpan OutTime { get; set; }
        [DisplayName("TotalWork Hours")]
        public float TotalTimeCount { get; set; }
        [DisplayName("Status")]

        public AttendanceStatus? UpdateStatus { get; set; }
        public string Notes { get; set; }


    }
}
