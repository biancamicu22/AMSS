using Microsoft.AspNetCore.Mvc;
using MMSAPI.Repository;
using System;
using DataLibrary.Models;
using AutoMapper;
using DataLibrary.DTO;
using System.Linq;
using MMSAPI.Validations;

namespace MMSAPI.Controllers
{
    [Route("api/v1/comment")]
    [ApiController]
    public class CommentController : Controller
    {
        private ICommentRepository _commentRepository;

        public IUserRepository UserRepository { get; }
        public IEntityUpdateHandler EntityUpdateHandler { get; }
        public ITaskRepository TaskRepository { get; }

        public CommentController(ICommentRepository commentRepository, IUserRepository userRepository,
            IEntityUpdateHandler entityUpdateHandler, ITaskRepository taskRepository)
        {
            _commentRepository = commentRepository;
            UserRepository = userRepository;
            EntityUpdateHandler = entityUpdateHandler;
            TaskRepository = taskRepository;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CommentDTO Comment)
        {
            var atra = Comment.ToModel();
            atra.Date_Posted = DateTime.Now;
            atra.Id = Guid.NewGuid().ToString();
            atra.User = UserRepository.GetById(User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value);
            atra.Task = TaskRepository.GetById(atra.Task_Id);

            if (atra.User == null || atra.Task == null) return BadRequest("Datele nu sunt valide");
            try
            {
                return Ok(CommentDTO.FromModel(_commentRepository.Add(atra)));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPut]
        public IActionResult Update([FromBody] CommentDTO Comment)
        {
            try
            {
                return EntityUpdateHandler.Update<Comment>(Comment.ToModel()).ToHttpResponse();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var comm = _commentRepository.GetById(id);
                if (comm.User_Id != User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value) return Unauthorized();
                return Ok(_commentRepository.Delete(id));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromQuery]string id)
        {
            try
            {
                return Ok(CommentDTO.FromModel(_commentRepository.GetById(id)));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var allComments = _commentRepository.GetAll();
                var commentsDTO = allComments.Select(c => CommentDTO.FromModel(c)).ToList();
                return Ok(commentsDTO);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
