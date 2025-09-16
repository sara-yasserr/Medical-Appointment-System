using AutoMapper;
using MedicalApp.BL.DTOs.DoctorDTOs;
using MedicalApp.BL.Interfaces;
using MedicalApp.DA.Interfaces;
using MedicalApp.DA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MedicalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public DoctorController( IUnitOfWork unitOfWork,
            IDoctorRepository doctorRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDoctorDTO registerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if email is already used
            var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (existingUser != null)
                return BadRequest("Email already used.");

            // Create Identity user
            var user = _mapper.Map<ApplicationUser>(registerDTO);

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            await _userManager.AddToRoleAsync(user, "Doctor");
            var doctor = _mapper.Map<Doctor>(registerDTO);
            doctor.UserId = user.Id;

            _unitOfWork.DoctorRepo.Add(doctor);
            _unitOfWork.SaveChanges();

            Log.Information("New doctor registered: {DoctorEmail}", doctor.User.Email);

            return Ok(new { Id = doctor.Id });
        }

        [Authorize(Roles = "Admin, Doctor")]
        [HttpGet("details/{id:int}")]
        public IActionResult GetDetails(int id)
        {
            var doctor = _unitOfWork.DoctorRepo.GetById(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return Ok(doctor);
        }
        [Authorize(Roles = "Admin, Doctor")]
        [HttpPatch("update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDoctorDTO updateDTO)
        {
            var doctor = _unitOfWork.DoctorRepo.GetById(id);
            if (doctor == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _mapper.Map(updateDTO, doctor);
            _mapper.Map(updateDTO, doctor.User);
            if (!string.IsNullOrEmpty(updateDTO.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(doctor.User);
                var result = await _userManager.ResetPasswordAsync(doctor.User, token, updateDTO.Password);
                if (!result.Succeeded)
                    return BadRequest(result.Errors);
            }
            _userManager.UpdateAsync(doctor.User).Wait();
            _unitOfWork.DoctorRepo.Update(doctor);
            _unitOfWork.SaveChanges();
            Log.Information("Doctor updated: Id={DoctorId}, Email={Email}", doctor.Id, doctor.User.Email);

            return Ok();
        }

    }
}
