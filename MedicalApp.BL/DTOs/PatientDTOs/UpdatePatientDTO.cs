using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalApp.BL.DTOs.PatientDTOs
{
    public class UpdatePatientDTO
    {
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters")]
        public string? FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters")]
        public string? LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20, ErrorMessage = "Phone number cannot be longer than 20 characters")]
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
    }
}
