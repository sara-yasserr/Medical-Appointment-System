using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MedicalApp.BL.DTOs.AppointmentDTOs;
using MedicalApp.BL.DTOs.DoctorDTOs;
using MedicalApp.BL.DTOs.PatientDTOs;
using MedicalApp.DA.Enums;
using MedicalApp.DA.Models;

namespace MedicalApp.BL.MapperConfig
{
    public class MappConfig : Profile
    {
        public MappConfig()
        {
            #region ApplicationUser Mappings
            CreateMap<RegisterDoctorDTO, ApplicationUser>().AfterMap((src, dest) =>
            {
                dest.UserName = src.Email;
            }).ReverseMap();
            CreateMap<RegisterPatientDTO, ApplicationUser>().AfterMap((src, dest) =>
            {
                dest.UserName = src.Email;
            }).ReverseMap();
            CreateMap<UpdateDoctorDTO, ApplicationUser>().AfterMap((src, dest) =>
            {
                if (!string.IsNullOrEmpty(src.Email))
                {
                    dest.UserName = src.Email;
                    dest.NormalizedUserName = src.Email.ToUpper();
                }
            })
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpdatePatientDTO, ApplicationUser>().AfterMap((src, dest) =>
            {
                if (!string.IsNullOrEmpty(src.Email))
                {
                    dest.UserName = src.Email;
                    dest.NormalizedUserName = src.Email.ToUpper();
                }
            })
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            #endregion

            #region Appointment Mappings
            CreateMap<Appointment, ReadAppointmentDTO>().AfterMap((src, dest) =>
            {
                dest.DoctorName = src.Doctor.User.FirstName + " " + src.Doctor.User.LastName;
                dest.PatientName = src.Patient.User.FirstName + " " + src.Patient.User.LastName;
                dest.DateOfBirth = src.Patient.DateOfBirth;
            }).ReverseMap();

            CreateMap<AppointmentDTO, Appointment>().AfterMap((src, dest) =>
            {
                dest.Status = (Status)Enum.Parse(typeof(Status),src.Status);
            }).ReverseMap();
            #endregion

            #region Doctor Mappings
            CreateMap<Doctor, RegisterDoctorDTO>().ReverseMap();
            CreateMap<UpdateDoctorDTO, Doctor>()
           .ForMember(dest => dest.Specialization, opt => opt.Condition(src => src.Specialization != null));
            CreateMap<Doctor, ReadDoctorDTO>().AfterMap((src, dest) =>
            {
                dest.FirstName = src.User.FirstName;
                dest.LastName = src.User.LastName;
                dest.PhoneNumber = src.User.PhoneNumber;
                dest.Email = src.User.Email;
            }).ReverseMap();
            #endregion

            #region Patient Mappings
            CreateMap<Patient, RegisterPatientDTO>().ReverseMap();
            CreateMap<UpdatePatientDTO, Patient>()
          .ForMember(dest => dest.DateOfBirth, opt => opt.Condition(src => src.DateOfBirth != null));
            CreateMap<Patient, ReadPatientDTO>().AfterMap((src, dest) =>
            {
                dest.FirstName = src.User.FirstName;
                dest.LastName = src.User.LastName;
                dest.PhoneNumber = src.User.PhoneNumber;
                dest.Email = src.User.Email;
            }).ReverseMap();
            #endregion

        }
    }
}
