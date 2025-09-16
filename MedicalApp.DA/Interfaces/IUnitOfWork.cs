using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalApp.DA.Models;
using Microsoft.AspNetCore.Identity;

namespace MedicalApp.DA.Interfaces
{
    public interface IUnitOfWork
    {
        //UserManager<ApplicationUser> UserManager { get; }
        IGenericRepository<Patient> PatientRepo { get; }
        IGenericRepository<Doctor> DoctorRepo { get; }
        IGenericRepository<Appointment> AppointmentRepo { get; }
        void SaveChanges();
    }
}
