using AutoMapper;
using MedicalApp.BL.DTOs.DoctorDTOs;
using MedicalApp.BL.DTOs.PatientDTOs;
using MedicalApp.BL.Interfaces;
using MedicalApp.DA.Interfaces;
using MedicalApp.DA.Models;
using MedicalApp.DA.Repositories.Custom;
using MedicalApp.DA.UnitOfWorks;
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
        public async Task<IActionResult> Register([FromBody] PatientDTO registerDTO)
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

        [HttpPatch("update/{id:int}")]
        public IActionResult Update(int id, [FromBody] PatientDTO updateDTO)
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
            _unitOfWork.PatientRepo.Update(patient);
            _unitOfWork.SaveChanges();
            Log.Information("Patient updated: Id={PatientId}, Email={Email}", patient.Id, patient.User.Email);
            return Ok();
        }
    }
}
