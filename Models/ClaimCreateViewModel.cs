using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ST10448420_CMCsystem.Models
{
    public class ClaimCreateViewModel
    {
        [Required, StringLength(10)]
        public string ClaimID { get; set; }

        [Required, StringLength(10)]
        public string LecturerID { get; set; }

        public string LecturerName { get; set; } // display only

        [Required, StringLength(50)]
        public string ClaimName { get; set; }

        [StringLength(255)]
        public string ClaimDescription { get; set; }

        [Required]
        public DateTime ClaimDate { get; set; } = DateTime.Today;

        [Required]
        [Range(0.01, double.MaxValue)]
        public double TotalHoursWorked { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public double HourlyRate { get; set; }

        public double TotalSalary => TotalHoursWorked * HourlyRate;

        [StringLength(50)]
        public string ClaimStatus { get; set; } = "Pending";

        public DateTime ClaimSubmissionDate { get; set; } = DateTime.Now;

        // file upload(s) handled via AJAX; store uploaded document IDs/client names here
        public List<UploadedFileInfo> UploadedFiles { get; set; } = new();

        // optional note field
        [StringLength(500)]
        public string AdditionalNotes { get; set; }
    }

    public class UploadedFileInfo
    {
        public string DocumentID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
