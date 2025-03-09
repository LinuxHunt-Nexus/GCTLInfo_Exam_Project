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

        [Required]
        public string Name { get; set; }
        public string DesignationID { get; set; }
        [Required]
        public string DesignationCode { get; set; }

        [ForeignKey("DesignationID")]
        public virtual Designation Designation { get; set; }
    }



}
