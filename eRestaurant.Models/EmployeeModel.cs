using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RocketPOS.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }

        [DisplayName("Employee Name")]
        [Required(ErrorMessage = "Enter First Name")]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Enter Last Name")]
        public string LastName { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AlterPhone { get; set; }
        public string PresentAdress { get; set; }
        public string PermanentAdress { get; set; }
        public string picture { get; set; }
        public string DegreeName { get; set; }
        public string UniversityName { get; set; }
        public string CGP { get; set; }
        public string PassingYear { get; set; }
        public string CompanyName { get; set; }
        public string WorkingPeriod { get; set; }
        public string Duties { get; set; }
        public string Suoervisor { get; set; }
        public string Signature { get; set; }
        public string State {get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string CitizenShip { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime OriginalHireDate { get; set; }
        public DateTime TerminationDate { get; set; }
        public string TerminationReason { get; set; }
        public bool VolunteryTermination { get; set; }
        public DateTime RehireDate { get; set; }
        public string RateType { get; set; }
        public decimal Rate { get; set; }
        public string PayFrequency { get; set; }
        public string PayFrequencyTxt { get; set; }
        public decimal HourlyRate2 { get; set; }
        public decimal HourlyRate3 { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string  Country { get; set; }
        public string MaritalStatus { get; set; }
        public string EthnicGroup { get; set; }
        public string SSN { get; set; }
        public string WorkInState { get; set; }
        public string LiveInState { get; set; }
        public string HomeEmail { get; set; }
        public string BusinessEmail { get; set; }
        public string HomePhone { get; set; }
        public string BUsinessPhone { get; set; }
        public string CellPhone { get; set; }
        public string EmergConct { get; set; }
        public string EmergHPhone { get; set; }
        public string EmergWPhone { get; set; }
        public string EmergContctRelation { get; set; }
        public string AltEmContct { get; set; }
        public string AltEmgHPhone { get; set; }
        public string AltEmgWPhone { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }

        public EmployeeModel()
        {
            IsActive = true;
        }
    }
}
