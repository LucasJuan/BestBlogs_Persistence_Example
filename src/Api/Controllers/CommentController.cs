using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
using Repository;

namespace Api.Controllers
{
    [ApiController]
    [Route("comments")]
    public class CommentController : ControllerBase
    {
        private readonly ILogger<CommentController> _logger;
        private readonly CommentRepository _repo;
        private readonly PostRepository _repoPost;

        public CommentController(ILogger<CommentController> logger, CommentRepository repo, PostRepository repoPost)
        {
            _logger = logger;
            _repo = repo;
            _repoPost = repoPost;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult<IEnumerable<Comment>> GetAll()
        {
            try
            {
                return Ok(_repo.GetAll().ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError("GetAll Error! Message: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "GetAll Error! Message: " + ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        [Route("GetId")]
        public ActionResult<Comment> Get([FromRoute] Guid id)
        {
            try
            {
                Comment comment = _repo.Get(id);

                if (!ModelState.IsValid)
                    return NotFound();

                return comment;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Get Error! Message: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("Post")]
        public ActionResult<Comment> Post([FromBody] Comment comment)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                //CHECK IF POST ALREADY EXISTS
                Post checkPost = _repoPost.Get(comment.PostId);

                if (checkPost == null)
                    return BadRequest("Post doesn't exist.");

                Comment newComment = _repo.Create(comment);

                return newComment;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Post Error! Message: " + ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        [Route("Put")]
        public IActionResult Put([FromRoute] Guid id, [FromBody] Comment comment)
        {
            try
            {
                if (id != comment.Id)
                    return BadRequest("Incorrect ID");

                Comment commentUpdate = _repo.Get(id);

                if (commentUpdate == null)
                    return NotFound($"Comment Id = {id} not found.");

                Comment updateComment = _repo.Update(comment);

                return Ok(updateComment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Put Error! Message: " + ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        [Route("Delete")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            try
            {
                Comment commentDelete = _repo.Get(id);

                if(commentDelete == null)
                    return NotFound($"Comment Id = {id} not found.");

                bool deleted = _repo.Delete(id);

                return deleted ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error! Message: " + ex.Message);
            }
        }
    }
}