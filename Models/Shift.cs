using System.ComponentModel.DataAnnotations;

namespace GCTLInfo_Exam_Project.Models
{
    public class Shift
    {
        [Key]
        [Required]
        public int ShiftCode { get; set; }

        [Required]
        public string ShiftName { get; set; }

        [Required]
        public string ShiftShortName { get; set; }

        [Required]
        public DateTime ShiftStartTime { get; set; }

        [Required]
        public DateTime ShiftEndTime { get; set; }

        [Required]
        public DateTime LateTime { get; set; }

        [Required]
        public DateTime AbsentTime { get; set; }

        [Required]
        public DateTime WEF { get; set; }

        public string Remarks { get; set; }

        public DateTime? EntryDate { get; set; }
        public DateTime? ModifyDate { get; set; }
    }

}
