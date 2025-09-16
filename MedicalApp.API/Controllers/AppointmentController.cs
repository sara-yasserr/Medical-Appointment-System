using AutoMapper;
using MedicalApp.BL.DTOs.AppointmentDTOs;
using MedicalApp.DA.Enums;
using MedicalApp.DA.Interfaces;
using MedicalApp.DA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MedicalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AppointmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [Authorize(Roles = "Patient")]
        [HttpPost("schedule-appointment")]
        public IActionResult CreateAppointment(AppointmentDTO appointmentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var appointment = _mapper.Map<Appointment>(appointmentDTO);
            _unitOfWork.AppointmentRepo.Add(appointment);
            _unitOfWork.SaveChanges();
            Log.Information("Appointment scheduled: PatientId={PatientId}, DoctorId={DoctorId}, Date={Date}",
             appointment.PatientId, appointment.DoctorId, appointment.AppointmentDate);

            return CreatedAtAction(nameof(GetById),new {id =appointment.Id}, appointmentDTO);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var appointment = _unitOfWork.AppointmentRepo.GetById(id);
            if (appointment == null)
            {
                return NotFound();
            }
            var readAppointmentDTO = _mapper.Map<ReadAppointmentDTO>(appointment);
            return Ok(readAppointmentDTO);
        }
        [Authorize(Roles = "Doctor")]
        [HttpGet("doctor/{doctorId:int}/appointments")]
        public IActionResult GetDoctorAppointments(int doctorId)
        {
            var appointments = _unitOfWork.AppointmentRepo.GetAll().Where(a => a.DoctorId == doctorId).ToList();
            var readAppointmentsDTO = _mapper.Map<List<ReadAppointmentDTO>>(appointments);
            return Ok(readAppointmentsDTO);
        }
        [Authorize(Roles = "Patient")]
        [HttpGet("patient/{patientId:int}/appointments")]
        public IActionResult GetPatientAppointments(int patientId)
        {
            var appointments = _unitOfWork.AppointmentRepo.GetAll().Where(a => a.PatientId == patientId).ToList();
            var readAppointmentsDTO = _mapper.Map<List<ReadAppointmentDTO>>(appointments);
            return Ok(readAppointmentsDTO);
        }
        [HttpPatch("cancel/{id:int}")]
        public IActionResult CancelAppointment(int id)
        {
            var appointment = _unitOfWork.AppointmentRepo.GetById(id);
            if (appointment == null)
            {
                return NotFound();  
            }
            appointment.Status = Status.Cancelled;
            _unitOfWork.AppointmentRepo.Update(appointment);
            _unitOfWork.SaveChanges();
            Log.Information("Appointment cancelled: AppointmentId={AppointmentId}, PatientId={PatientId}, DoctorId={DoctorId},Date={Date}",
                appointment.Id, appointment.PatientId, appointment.DoctorId, appointment.AppointmentDate);
            return NoContent();
        }
    }
}
