using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCTLInfo_Exam_Project.Models
{
    public class Employee
    {
        [Key]
        public int AI_ID { get; set; }
        [Required]
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        [Required]
        public string DesignationCode { get; set; } // এটা string, তাই Designation এর Primary Key ও string হতে হবে

        [ForeignKey("DesignationCode")]
        public virtual Designation Designation { get; set; }
    }

}
