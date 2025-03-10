using System.ComponentModel.DataAnnotations;

namespace GCTLInfo_Exam_Project.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        // Customer fields
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }
        public DateTime? BusinessStartDate { get; set; }
        public string CustomerType { get; set; }
        public decimal? CreditLimit { get; set; }
        public string? Photo { get; set; }

        // Navigation property for Delivery Addresses
        public ICollection<DeliveryInfo> DeliveryAddresses { get; set; } = new List<DeliveryInfo>();
    }


}
