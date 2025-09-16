using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalApp.DA.Interfaces;
using MedicalApp.DA.Models;
using MedicalApp.DA.Repositories;
using Microsoft.AspNetCore.Identity;

namespace MedicalApp.DA.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        //private readonly UserManager<ApplicationUser> _userManager;
        //public RoleManager<IdentityRole> _roleManager;
        private IGenericRepository<Patient> _patientRepo;
        private IGenericRepository<Doctor> _doctorRepo;
        private IGenericRepository<Appointment> _appointmentRepo;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        //public UserManager<ApplicationUser> UserManager => _userManager;
        public IGenericRepository<Patient> PatientRepo
        {
            get
            {
                if(_patientRepo == null)
                {
                    _patientRepo = new GenericRepository<Patient>(_context);
                }
                return _patientRepo;
            }
        }
        public IGenericRepository<Doctor> DoctorRepo
        {
            get
            {
                if (_doctorRepo == null)
                {
                    _doctorRepo = new GenericRepository<Doctor>(_context);
                }
                return _doctorRepo;
            }
        }
        public IGenericRepository<Appointment> AppointmentRepo
        {
            get
            {
                if (_appointmentRepo == null)
                {
                    _appointmentRepo = new GenericRepository<Appointment>(_context);
                }
                return _appointmentRepo;
            }
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
