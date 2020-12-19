namespace RocketPOS.Models
{
    public class SupplierModel
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public int TaxType { get; set;}
        public string SupplierAddress1 { get; set; }
        public string SupplierAddress2 { get; set;}
        public string SupplierPhone { get; set; }
        public string SupplierPicture { get; set; }
        public string SupplierEmail { get; set; }
        public bool IsActive { get; set; }
        public int  UserId { get; set; }
    }
}
