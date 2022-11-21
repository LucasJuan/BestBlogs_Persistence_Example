using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
using Repository;

namespace Api.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private readonly PostRepository _repo;
        private readonly CommentRepository _repoCom;

        public PostController(ILogger<PostController> logger, PostRepository repo, CommentRepository repoCom)
        {
            _logger = logger;
            _repo = repo;
            _repoCom = repoCom;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult<IEnumerable<Post>> GetAll()
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
        [Route("Get")]
        public ActionResult<Post> Get([FromRoute] Guid id)
        {
            try
            {
                Post post = _repo.Get(id);

                if (post == null)
                    return NotFound();

                return post;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Get Error! Message: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("Post")]
        public ActionResult<Post> Post([FromBody] Post post)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                Post newPost = _repo.Create(post);

                return newPost;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Post Error! Message: " + ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        [Route("Put")]
        public ActionResult<Post> Put([FromRoute] Guid id, [FromBody] Post post)
        {
            try
            {
                if (id != post.Id)
                    return BadRequest("Incorrect ID");

                Post postUpdate = _repo.Get(id);

                if (postUpdate == null)
                    return NotFound("Comment Id = {id} not found.");

                Post updatedPost = _repo.Update(post);

                return Ok(updatedPost);
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
                Post postDelete = _repo.Get(id);

                if (postDelete == null)
                    return NotFound("Comment Id = {id} not found.");

                bool deleted = _repo.Delete(id);

                return deleted ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error! Message: " + ex.Message);
            }
        }

        [HttpGet("{id:guid}/comments")]
        [Route("GetComments")]
        public ActionResult<IEnumerable<Comment>> GetComments([FromRoute] Guid id)
        {
            try
            {
                IEnumerable <Comment> comments = _repoCom.GetByPostId(id);

                return comments.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Get Error! Message: " + ex.Message);
            }
        }
    }
}