using AutoMapper;
using MedicalApp.BL.DTOs.DoctorDTOs;
using MedicalApp.BL.DTOs.PatientDTOs;
using MedicalApp.BL.Interfaces;
using MedicalApp.DA.Interfaces;
using MedicalApp.DA.Models;
using MedicalApp.DA.Repositories.Custom;
using MedicalApp.DA.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MedicalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public PatientController(IUnitOfWork unitOfWork,
            IPatientRepository patientRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            //_patientRepository = patientRepository;
            _mapper = mapper;
            _userManager = userManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterPatientDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Check if email is already used
            var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (existingUser != null)
                return BadRequest("Email already used.");

            // Create Identity user
            var user = _mapper.Map<ApplicationUser>(registerDTO);

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            await _userManager.AddToRoleAsync(user, "Patient");
            var patient = _mapper.Map<Patient>(registerDTO);
            patient.UserId = user.Id;
            
            _unitOfWork.PatientRepo.Add(patient);
            _unitOfWork.SaveChanges();

            Log.Information("New patient registered: {PatientEmail}", patient.User.Email);

            return Ok(new { Id = patient.Id });
        }

        [Authorize(Roles = "Admin, Patient")]
        [HttpGet("details/{id:int}")]
        public IActionResult GetDetails(int id)
        {
            var patient = _unitOfWork.PatientRepo.GetById(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [Authorize(Roles = "Admin, Patient")]
        [HttpPatch("update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePatientDTO updateDTO)
        {
            var patient = _unitOfWork.PatientRepo.GetById(id);
            if(patient == null){
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _mapper.Map(updateDTO, patient);
            _mapper.Map(updateDTO, patient.User);
            if (!string.IsNullOrEmpty(updateDTO.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(patient.User);
                var result = await _userManager.ResetPasswordAsync(patient.User, token, updateDTO.Password);
                if (!result.Succeeded)
                    return BadRequest(result.Errors);
            }
            _unitOfWork.PatientRepo.Update(patient);
            _userManager.UpdateAsync(patient.User).Wait();
            _unitOfWork.SaveChanges();
            Log.Information("Patient updated: Id={PatientId}, Email={Email}", patient.Id, patient.User.Email);
            return Ok();
        }
    }
}
