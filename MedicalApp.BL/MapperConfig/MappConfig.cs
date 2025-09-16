using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MedicalApp.BL.DTOs.AppointmentDTOs;
using MedicalApp.BL.DTOs.DoctorDTOs;
using MedicalApp.BL.DTOs.PatientDTOs;
using MedicalApp.DA.Models;

namespace MedicalApp.BL.MapperConfig
{
    public class MappConfig:Profile
    {
        public MappConfig() 
        {
            #region ApplicationUser Mappings
            CreateMap<ApplicationUser, DoctorDTO>().ReverseMap();
            CreateMap<ApplicationUser, PatientDTO>().ReverseMap();
            #endregion

            #region Appointment Mappings
            CreateMap<Appointment, ReadAppointmentDTO>().AfterMap((src, dest) =>
            {
                dest.DoctorName = src.Doctor.User.FirstName + " " + src.Doctor.User.LastName;
                dest.PatientName = src.Patient.User.FirstName + " " + src.Patient.User.LastName;

            }).ReverseMap();

            #endregion

            #region Doctor Mappings
            CreateMap<Doctor, DoctorDTO>().ReverseMap();
            #endregion

            #region Patient Mappings
            CreateMap<Patient, PatientDTO>().ReverseMap();
            #endregion

        }
    }
}
