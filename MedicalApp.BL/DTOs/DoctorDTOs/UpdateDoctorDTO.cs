using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalApp.BL.DTOs.DoctorDTOs
{
    public class UpdateDoctorDTO
    {
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters")]
        public string? FirstName { get; set; }
        [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters")]
        public string? LastName { get; set; }
        [StringLength(100, ErrorMessage = "Specialization cannot be longer than 100 characters")]
        public string? Specialization { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address format")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number format")]
        [StringLength(15, ErrorMessage = "Phone Number cannot be longer than 15 digits")]
        public string? PhoneNumber { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
