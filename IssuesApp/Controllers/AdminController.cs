﻿using AutoMapper;
using IssuesApp.Dto;
using IssuesApp.Interfaces;
using IssuesApp.Models;
using IssuesApp.Repositories;
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
        public AdminController(ILogger<AdminController> logger, IAdminRepository adminRepository, IMapper mapper) 
        {
            _logger = logger;
            _adminRepository = adminRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Admin>))]
        public IActionResult GetAdmins() 
        {
            var admins = _mapper.Map<List<AdminDto>>(_adminRepository.GetAdmins());
            if (!ModelState.IsValid)
            {
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
            _logger.LogInformation("Getting admin {Id}", adminId);

            if (!_adminRepository.AdminExists(adminId))
            {
                return NotFound();
            }
            var admin = _mapper.Map<AdminDto>(_adminRepository.GetAdmin(adminId));
            if(!ModelState.IsValid)
            {
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
                return BadRequest(ModelState);

            if (!_adminRepository.DeleteAdmin(adminToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting admin");
            }

            return NoContent();
        }
    }
}