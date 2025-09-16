using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalApp.BL.Interfaces;
using MedicalApp.DA.Models;

namespace MedicalApp.DA.Repositories.Custom
{
    public class PatientRepository :IPatientRepository
    {
        private readonly AppDbContext _context;
        public PatientRepository(AppDbContext context)
        {
            _context = context;
        }
        //public Patient? GetByEmail(string email)
        //{
        //    return _context.Set<Patient>().FirstOrDefault(c => c.Email == email);
        //}
    }
}
