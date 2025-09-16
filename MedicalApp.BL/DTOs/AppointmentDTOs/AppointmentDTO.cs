using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalApp.DA.Enums;
using MedicalApp.DA.Models;

namespace MedicalApp.BL.DTOs.AppointmentDTOs
{
    public class AppointmentDTO
    {

        [Required(ErrorMessage = "Appointment Date is required")]
        [DataType(DataType.DateTime)]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Reason is required")]
        [StringLength(250, ErrorMessage = "Reason cannot be longer than 250 characters")]
        public string Reason { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Patient is required")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Doctor is required")]
        public int DoctorId { get; set; }
    }
}
