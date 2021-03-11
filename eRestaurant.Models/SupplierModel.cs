using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RocketPOS.Models
{
    public class SupplierModel
    {
        public int Id { get; set; }

        [DisplayName("Supplier")]
        [Required(ErrorMessage = "Enter Supplier Name")]
        public string SupplierName { get; set; }
        [DisplayName("Tax Inclusive")]
        public bool TaxType { get; set;}
        [DisplayName("Address")]
        public string SupplierAddress1 { get; set; }
        public string SupplierAddress2 { get; set;}
        [DisplayName("Phone")]
        public string SupplierPhone { get; set; }
        public string SupplierPicture { get; set; }
        [DisplayName("Email")]
        public string SupplierEmail { get; set; }
        public bool IsActive { get; set; }
        public int  UserId { get; set; }
        [DisplayName("City")]
        public string City { get; set; }
        [DisplayName("Contact Person")]
        public string ContactPerson { get; set; }
        [DisplayName("Vat")]
        public string VATNumber { get; set; }

        [DisplayName("Pin")]
        public string PINNumber { get; set; }

        public SupplierModel()
        {
            IsActive = true;
        }
    }
}
