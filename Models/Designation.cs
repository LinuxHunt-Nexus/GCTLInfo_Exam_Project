using System.ComponentModel.DataAnnotations;

namespace GCTLInfo_Exam_Project.Models
{
    public class Designation
    {
        [Key]
        public int AI_ID { get; set; }
        public string DesignationCode { get; set; }
        public string DesignationName { get; set; }
        public string DesignationShortName { get; set; }
    }
}
