﻿using GCTLInfo_Exam_Project.Models;
using System.ComponentModel.DataAnnotations;


namespace GCTLInfo_Exam_Project.viewModels
{
    public class RosterScheduleEntryViewModel
    {
        public decimal AI_ID { get; set; }
        public string EmployeeID { get; set; }
        public string Remarks { get; set; }
        public List<EmployeeViewModel> Employees { get; set; }
        public List<Shift> Shifts { get; set; }
        public List<string> SelectedEmployees { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        [Required]
        public DateTime DateTo { get; set; }
        public int ShiftCode { get; set; }
    }

}
