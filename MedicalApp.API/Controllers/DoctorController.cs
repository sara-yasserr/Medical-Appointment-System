using AutoMapper;
using MedicalApp.BL.DTOs.DoctorDTOs;
using MedicalApp.BL.Interfaces;
using MedicalApp.DA.Interfaces;
using MedicalApp.DA.Models;
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
        public DoctorController( IUnitOfWork unitOfWork,
            IDoctorRepository doctorRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DoctorDTO registerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if email is already used
            var existingUser = await _unitOfWork.UserManager.FindByEmailAsync(registerDTO.Email);
            if (existingUser != null)
                return BadRequest("Email already used.");

            // Create Identity user
            var user = _mapper.Map<ApplicationUser>(registerDTO);

            var result = await _unitOfWork.UserManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            await _unitOfWork.UserManager.AddToRoleAsync(user, "Doctor");
            var doctor = _mapper.Map<Doctor>(registerDTO);
            doctor.UserId = user.Id; 

            _unitOfWork.DoctorRepo.Add(doctor);
            _unitOfWork.SaveChanges();

            Log.Information("New doctor registered: {DoctorEmail}", doctor.User.Email);

            return Ok(new { Id = doctor.Id });
        }


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

        [HttpPatch("update/{id:int}")]
        public IActionResult Update(int id, [FromBody] DoctorDTO updateDTO)
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
            _unitOfWork.DoctorRepo.Update(doctor);
            _unitOfWork.SaveChanges();
            Log.Information("Doctor updated: Id={DoctorId}, Email={Email}", doctor.Id, doctor.User.Email);

            return Ok();
        }

    }
}
