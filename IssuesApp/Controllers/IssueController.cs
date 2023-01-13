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
    public class IssueController : Controller
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IMapper _mapper;
        private readonly IMessageProducer _messagePublisher;
        public IssueController(IIssueRepository issueRepository, IMapper mapper, IMessageProducer messagePublisher)
        {
            _issueRepository = issueRepository;
            _mapper = mapper;
            _messagePublisher = messagePublisher;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Issue>))]
        public IActionResult GetIssues()
        {
            var issues = _mapper.Map<List<IssueDto>>(_issueRepository.GetIssues());
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(issues);
        }

        [HttpGet("{issueId}")]
        [ProducesResponseType(200, Type = typeof(Issue))]
        [ProducesResponseType(400)]
        public IActionResult GetIssue(int issueId)
        {
            if (!_issueRepository.IssueExists(issueId))
            {
                return NotFound();
            }
            var issue = _mapper.Map<IssueDto>(_issueRepository.GetIssue(issueId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(issue);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public IActionResult CreateIssue([FromBody] IssueDto issueCreate)
        {
            if (issueCreate == null)
            {
                return BadRequest(ModelState);
            }

            var issue = _issueRepository.GetIssues()
                .Where(a => a.Title == issueCreate.Title)
                .FirstOrDefault();

            if (issue != null)
            {
                ModelState.AddModelError("", "Issue already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var issueMap = _mapper.Map<Issue>(issueCreate);
            if (!_issueRepository.CreateIssue(issueMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving!");
                return StatusCode(500, ModelState);
            }

            _messagePublisher.SendMessage(issueMap);

            return Ok("Successfully added the issue!");
        }

        [HttpPut("{issueId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateIssue(int issueId, [FromBody] IssueDto updatedIssue)
        {
            if (updatedIssue == null)
            {
                return BadRequest(ModelState);
            }

            if (issueId != updatedIssue.IssueId)
            {
                return BadRequest(ModelState);
            }

            if (!_issueRepository.IssueExists(issueId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var issueMap = _mapper.Map<Issue>(updatedIssue);

            if (!_issueRepository.UpdateIssue(issueMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating the issue");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{issueId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteIssue(int issueId)
        {
            if (!_issueRepository.IssueExists(issueId))
            {
                return NotFound();
            }

            var issueToDelete = _issueRepository.GetIssue(issueId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_issueRepository.DeleteIssue(issueToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting issue");
            }

            return NoContent();
        }
    }
}
