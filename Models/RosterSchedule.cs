using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCTLInfo_Exam_Project.Models
{
    public class RosterSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal AI_ID { get; set; }

        [Required]
        public string RosterScheduleCode { get; set; }

        [Required(ErrorMessage = "Employee ID is required.")]
        public string EmployeeID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "ShiftCode is required.")]
        public int ShiftCode { get; set; }

        [Required]
        public string Remarks { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee? Employee { get; set; }

        [ForeignKey("ShiftCode")]
        public virtual Shift? Shift { get; set; }
    }



}
