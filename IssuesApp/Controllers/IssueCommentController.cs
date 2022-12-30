using AutoMapper;
using IssuesApp.Dto;
using IssuesApp.Interfaces;
using IssuesApp.Models;
using IssuesApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IssuesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueCommentController : Controller
    {
        private readonly IIssueCommentRepository _issueCommentRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly IMapper _mapper;
        public IssueCommentController(IIssueCommentRepository issueCommentRepository, IAdminRepository adminRepository, IIssueRepository issueRepository, IMapper mapper) 
        {
            _issueCommentRepository = issueCommentRepository;
            _adminRepository = adminRepository;
            _issueRepository = issueRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<IssueComment>))]
        public IActionResult GetAllComments()
        {
            var comments = _mapper.Map<List<IssueCommentDto>>(_issueCommentRepository.GetComments());
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(comments);
        }

        [HttpGet("{commentId}")]
        [ProducesResponseType(200, Type = typeof(IssueComment))]
        [ProducesResponseType(400)]
        public IActionResult GetComment(int commentId)
        {
            if(!_issueCommentRepository.CommentExists(commentId))
            {
                return NotFound();
            }
            var comment = _mapper.Map<IssueCommentDto>(_issueCommentRepository.GetComment(commentId));
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(comment);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public IActionResult AddComment([FromBody] IssueCommentDto commentCreate)
        {
            if (commentCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!_adminRepository.AdminExists(commentCreate.AdminId))
            {
                return NotFound("Admin does not Exist!");
            }

            if (!_issueRepository.IssueExists(commentCreate.IssueId))
            {
                return NotFound("Issue does not Exist!");
            }

            var issuecomment = _issueCommentRepository.GetComments()
                .Where(a => a.CommentText == commentCreate.CommentText && a.IssueId == commentCreate.IssueId && a.AdminId == commentCreate.AdminId)
                .FirstOrDefault();

            if (issuecomment != null)
            {
                ModelState.AddModelError("", "Comment already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var issueCommentMap = _mapper.Map<IssueComment>(commentCreate);
            if (!_issueCommentRepository.CreateComment(issueCommentMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving!");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully added the comment!");
        }

        [HttpPut("{commentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateComment(int commentId, [FromBody] IssueCommentDto updatedComment)
        {
            if (updatedComment == null)
            {
                return BadRequest(ModelState);
            }

            if (commentId != updatedComment.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_issueCommentRepository.CommentExists(commentId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var issueCommentMap = _mapper.Map<IssueComment>(updatedComment);

            if (!_issueCommentRepository.UpdateComment(issueCommentMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating comment");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{commentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteComment(int commentId)
        {
            if (!_issueCommentRepository.CommentExists(commentId))
            {
                return NotFound();
            }

            var categoryToDelete = _issueCommentRepository.GetComment(commentId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_issueCommentRepository.DeleteComment(categoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting comment");
            }

            return NoContent();
        }
    }
}
