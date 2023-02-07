using Microsoft.AspNetCore.Mvc;
using MMSAPI.Repository;
using System;
using DataLibrary.Models;
using AutoMapper;
using DataLibrary.DTO;
using DataLibrary.StatePattern;
using System.Linq;
using MMSAPI.Validations;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MMSAPI.Controllers
{
    [Route("api/v1/task")]
    [ApiController]
    public class TaskController : Controller
    {
        private ITaskRepository _taskRepository;
        private ISprintRepository _sprintRepository;
        private IUserRepository _userRepository;

        public IEntityUpdateHandler _entityUpdateHandler { get; }

        public TaskController(ITaskRepository taskRepository, ISprintRepository sprintRepository, IUserRepository userRepository,
            IEntityUpdateHandler entityUpdateHandler)
        {
            _taskRepository = taskRepository;
            _sprintRepository = sprintRepository;
            _userRepository = userRepository;
            _entityUpdateHandler = entityUpdateHandler;
        }

        [HttpPost]
        public IActionResult Create([FromBody] TaskDTO task)
        {
            try
            {
                return Ok(TaskDTO.FromModel(_taskRepository.Add(GenerateTask(task))));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public AppTask GenerateTask(TaskDTO task)
        {
            var atra = task.ToModel();
            atra.State = new AssignedState();
            atra.Id = Guid.NewGuid().ToString();
            atra.User = _userRepository.GetById(atra.User_Id);
            atra.Sprint = _sprintRepository.GetById(task.Sprint_id);
            atra.State.Change(atra);
            _taskRepository.AssignTaskEmail(atra);
            return atra;
        }

        [HttpPut]
        public IActionResult Update([FromBody] TaskDTO task)
        {
            try
            {
                return _entityUpdateHandler.Update<AppTask>(task.ToModel()).ToHttpResponse();
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
                return Ok(_taskRepository.Delete(id));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                return Ok(TaskDTO.FromModel(_taskRepository.GetById(id)));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("all/{sprintid}")]
        public IActionResult GetAll(string sprintid)
        {
            try
            {
                return Ok(_taskRepository.GetAll().Where(t => t.Sprint_Id.Equals(sprintid)).Select(s => TaskDTO.FromModel(s)));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_taskRepository.GetAll().Select(t => TaskDTO.FromModel(t)));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [ExcludeFromCodeCoverage]
        [HttpPut("done/{taskID}")]
        public void CompleteTask([FromRoute] string taskID)
        {
            _taskRepository.CompleteTask(taskID);

        }
        [ExcludeFromCodeCoverage]
        [HttpPut("inprogress/{taskID}")]
        public void StartTask([FromRoute] string taskID)
        {
            _taskRepository.StatTask(taskID);
        }
        [ExcludeFromCodeCoverage]
        [HttpPut("todo/{taskID}")]
        public void ToDoTask([FromRoute] string taskID)
        {
            _taskRepository.AssignTask(taskID);
        }
    }
}
