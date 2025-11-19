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
        [Range(5, 195, ErrorMessage = "Hours worked must be between 5 and 195 hours per month.")]
        public decimal TotalHoursWorked { get; set; }


        [Required]
        [Range(typeof(decimal), "28.79", "9999", ErrorMessage = "Hourly rate must be at least R28.79.")]
        public decimal HourlyRate { get; set; }

        // Auto-calculated total
        [NotMapped]
        public decimal TotalSalary => (decimal)TotalHoursWorked * HourlyRate;

        [StringLength(50)]
        public string ClaimStatus { get; set; } = "Pending";

        public DateTime ClaimSubmissionDate { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string AdditionalNotes { get; set; }

        // Uploads before saving
        public List<IFormFile> SupportingDocuments { get; set; } = new();

        // Saved metadata
        public List<UploadedFileDto> UploadedFiles { get; set; } = new();
    }

    public class UploadedFileDto
    {
        public string DocumentID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
