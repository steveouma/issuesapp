using AutoMapper;
using IssuesApp.Dto;
using IssuesApp.Interfaces;
using IssuesApp.Models;
using IssuesApp.Repositories;
using IssuesApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace IssuesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly IMessageProducer _messagePublisher;
        public AdminController(ILogger<AdminController> logger, IAdminRepository adminRepository, IMapper mapper, IMessageProducer messagePublisher) 
        {
            _logger = logger;
            _adminRepository = adminRepository;
            _mapper = mapper;
            _messagePublisher = messagePublisher;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Admin>))]
        public IActionResult GetAdmins() 
        {
            var admins = _mapper.Map<List<AdminDto>>(_adminRepository.GetAdmins());
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Bad request detected");
                return BadRequest(ModelState);
            }
            return Ok(admins);
        }

        [HttpGet("{adminId}")]
        [ProducesResponseType(200, Type = typeof(Admin))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetAdmin(int adminId)
        {
            if (!_adminRepository.AdminExists(adminId))
            {
                _logger.LogInformation("Trying to get non-existent admin {Id}", adminId);
                return NotFound();
            }
            var admin = _mapper.Map<AdminDto>(_adminRepository.GetAdmin(adminId));
            if(!ModelState.IsValid)
            {
                _logger.LogError("Bad request received while trying to get non-existent admin {Id}", adminId);
                return BadRequest(ModelState);
            }
            return Ok(admin);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public IActionResult CreateAdmin([FromBody] AdminDto adminCreate)
        {
            if(adminCreate == null)
            {
                return BadRequest(ModelState);
            }
            
            var admin = _adminRepository.CheckAdminEmail(adminCreate);

            if(admin!=null)
            {
                ModelState.AddModelError("", "Admin already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var adminMap = _mapper.Map<Admin>(adminCreate);
            if (!_adminRepository.CreateAdmin(adminMap))
            {
                _logger.LogError("Error occured while creating a new admin");
                ModelState.AddModelError("", "Something went wrong while saving!");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created the admin!");
        }

        [HttpPut("{adminId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateAdmin(int adminId, [FromBody] AdminDto updatedAdmin)
        {
            if (updatedAdmin == null)
            {
                return BadRequest(ModelState);
            }

            if (adminId != updatedAdmin.AdminId)
            {
                return BadRequest(ModelState);
            }

            if (!_adminRepository.AdminExists(adminId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var adminMap = _mapper.Map<Admin>(updatedAdmin);

            if (!_adminRepository.UpdateAdmin(adminMap))
            {
                _logger.LogError("Error occured while updating an admin");
                ModelState.AddModelError("", "Something went wrong while updating the admin");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{adminId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteAdmin(int adminId)
        {
            if (!_adminRepository.AdminExists(adminId))
            {
                return NotFound();
            }

            var adminToDelete = _adminRepository.GetAdmin(adminId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_adminRepository.DeleteAdmin(adminToDelete))
            {
                _logger.LogError("Error occured while deleting an admin");
                ModelState.AddModelError("", "Something went wrong deleting admin");
            }

            return NoContent();
        }
    }
}
