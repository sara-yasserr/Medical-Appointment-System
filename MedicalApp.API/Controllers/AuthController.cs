using MedicalApp.BL.DTOs.AuthDTOs;
using MedicalApp.BL.Services;
using MedicalApp.DA.Interfaces;
using MedicalApp.DA.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MedicalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IUnitOfWork unitOfWork, AuthService authService ,UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _userManager = userManager;

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid Request Data" });

            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
                return NotFound(new { message = "User not found" });

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!isPasswordValid)
                return Unauthorized(new { message = "Invalid username or password" });

            var roles = await _userManager.GetRolesAsync(user);

            var token = _authService.GenerateToken(user, roles);

            return Ok(new { token });
        }
    }
}
