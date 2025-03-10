using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCTLInfo_Exam_Project.Models
{
    public class DeliveryInfo
    {
        [Key]
        public int DeliveryID { get; set; }
        public string DeliveryAddress { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }

        // Foreign Key for Customer relationship
        public int? CustomerRefId { get; set; }

        // Navigation property to link back to Customer
        [ForeignKey("CustomerRefId")]
        public Customer? Customer { get; set; }
    }

}
