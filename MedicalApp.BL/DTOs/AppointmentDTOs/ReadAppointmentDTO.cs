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
    public class ReadAppointmentDTO
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; }
        public Status Status { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
    }
}
