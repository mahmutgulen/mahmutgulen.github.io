using FranchiseMenu.BLL.Abstract;
using FranchiseMenu.ENTITY.Dtos.AuthDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FranchiseMenu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registerformanageradmin")]
        public IActionResult RegisterForManagerAdmin(RegisterForManagerDto dto)
        {
            var result = _authService.RegisterForManagerAdmin(dto);
            return Ok(result);
        }

        [HttpPost("adminlogin")]
        public IActionResult AdminLogin(AdminLoginDto dto)
        {
            var result = _authService.AdminLogin(dto);
            return Ok(result);
        }

        [HttpPost("adminpasswordcahnge")]
        public IActionResult AdminPasswordChange(AdminPasswordChangeDto dto)
        {
            var result = _authService.AdminPasswordChange(dto);
            return Ok(result);
        }
    }
}
