using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyProject.Model.Database;
using MyProject.Model.DTOs;
using MyProject.Model.Services;
using static MyProject.Model.DTOs.UserDTO;

namespace MyProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IDatabaseService _service;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IDatabaseService service)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _service = service;
        }

        //api/User/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, false, false);
            if (result.Succeeded)
            {
                return Ok(_service.GetUserByUserName(login.UserName).Id);
            }
            return Unauthorized("Login failed");
        }

        //GET: api/User
        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> GetUsers() => _service.GetAllUser().Select(u => (UserDTO)u).ToList();

        //GET: api/User/1
        [HttpGet("{id}")]
        public ActionResult<UserDTO> GetUserById(int id) => (UserDTO)_service.GetUser(id);

        //GET: api/User/1/npus
        [HttpGet("{id}/npus")]
        public ActionResult<IEnumerable<NpuDTO>> GetUserUploadedNpus(int id) => _service.GetUserNpus(id).Select(w => (NpuDTO)w).ToList();
    }
}
