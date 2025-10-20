using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10448420_CMCsystem.Models
{
    public class ClaimCreateViewModel
    {
        [StringLength(10)]
        public string ClaimID { get; set; }

        [Required, StringLength(10)]
        public string LecturerID { get; set; }

        public string LecturerName { get; set; } // Display only

        [Required, StringLength(50)]
        public string ClaimName { get; set; }

        [StringLength(255)]
        public string ClaimDescription { get; set; }

        [Required]
        public DateTime ClaimDate { get; set; } = DateTime.Today;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Enter valid hours.")]
        public double TotalHoursWorked { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Enter valid hourly rate.")]
        public double HourlyRate { get; set; }

        [NotMapped]
        public double TotalSalary => TotalHoursWorked * HourlyRate;

        [StringLength(50)]
        public string ClaimStatus { get; set; } = "Pending";

        public DateTime ClaimSubmissionDate { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string AdditionalNotes { get; set; }

        //Handles uploaded files before saving
        [Display(Name = "Supporting Documents")]
        public List<IFormFile> SupportingDocuments { get; set; } = new();

        // Stores file info after upload
        public List<UploadedFileDto> UploadedFiles { get; set; } = new();
    }

    // DTO for uploaded files (metadata only)
    public class UploadedFileDto
    {
        public string DocumentID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
