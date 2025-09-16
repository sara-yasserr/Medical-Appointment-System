using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalApp.BL.Interfaces;
using MedicalApp.DA.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalApp.DA.Repositories.Custom
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly AppDbContext _context;
        public DoctorRepository(AppDbContext context)
        {
            _context = context;
        }
        //public Doctor? GetByEmail(string email)
        //{
        //    return _context.Set<Doctor>().FirstOrDefault(c => c.Email == email);
        //}
    }
}
