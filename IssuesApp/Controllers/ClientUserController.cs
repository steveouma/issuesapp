using AutoMapper;
using IssuesApp.Dto;
using IssuesApp.Interfaces;
using IssuesApp.Models;
using IssuesApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IssuesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientUserController : Controller
    {
        private readonly IClientUserRepository _clientUserRepository;
        private readonly IMapper _mapper;
        public ClientUserController(IClientUserRepository clientUserRepository, IMapper mapper) 
        {
            _clientUserRepository = clientUserRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ClientUser>))]
        public IActionResult GetClientUsers() 
        {
            var clientusers = _mapper.Map<List<ClientUserDto>>(_clientUserRepository.GetClientUsers());
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(clientusers);
        }

        [HttpGet("{clientId}")]
        [ProducesResponseType(200, Type = typeof(ClientUser))]
        [ProducesResponseType(400)]
        public IActionResult GetClient(int clientId)
        {
            if(!_clientUserRepository.ClientUserExists(clientId))
            {
                return NotFound();
            }
            var clientuser = _mapper.Map<ClientUserDto>(_clientUserRepository.GetClientUser(clientId));
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(clientuser);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public IActionResult CreateClientUser([FromBody] ClientUserDto clientUserCreate)
        {
            if(clientUserCreate== null)
            {
                return BadRequest(ModelState);
            }

            var clientuser = _clientUserRepository.GetClientUsers()
                .Where(cl=>cl.Email == clientUserCreate.Email)
                .FirstOrDefault();

            if(clientuser!=null)
            {
                ModelState.AddModelError("", "Client User already exists");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clientUserMap = _mapper.Map<ClientUser>(clientUserCreate);
            if(!_clientUserRepository.CreateClientUser(clientUserMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created the client user!");
        }

        [HttpPut("{clientId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateClientUser(int clientId, [FromBody] ClientUserDto updatedClientUser)
        {
            if (updatedClientUser == null)
            {
                return BadRequest(ModelState);
            }

            if (clientId != updatedClientUser.ClientUserId)
            {
                return BadRequest(ModelState);
            }

            if (!_clientUserRepository.ClientUserExists(clientId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var clientUserMap = _mapper.Map<ClientUser>(updatedClientUser);

            if (!_clientUserRepository.UpdateClientUser(clientUserMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating the admin");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{clientId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteClientUser(int clientId)
        {
            if (!_clientUserRepository.ClientUserExists(clientId))
            {
                return NotFound();
            }

            var clientUserToDelete = _clientUserRepository.GetClientUser(clientId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_clientUserRepository.DeleteClientUser(clientUserToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting admin");
            }

            return NoContent();
        }
    }
}
