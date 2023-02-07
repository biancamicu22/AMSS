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
    [Route("api/v1/sprint")]
    [ApiController]
    public class SprintController : Controller
    {
        private ISprintRepository _sprintRepository;
        private IMapper _autoMapper;

        public IEntityUpdateHandler EntityUpdateHandler { get; }

        public SprintController(ISprintRepository sprintRepository, IMapper autoMapper, IEntityUpdateHandler entityUpdateHandler)
        {
            _sprintRepository = sprintRepository;
            _autoMapper = autoMapper;
            EntityUpdateHandler = entityUpdateHandler;
        }

        [HttpPost]
        public IActionResult Create([FromBody] SprintDTO sprint)
        {
            try
            {
                var atra = sprint.ToModel();
                atra.Id = Guid.NewGuid().ToString();
                return Ok(SprintDTO.FromModel(_sprintRepository.Add(atra)));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPut]
        public IActionResult Update([FromBody] SprintDTO sprint)
        {
            try
            {
                if (sprint == null) throw new ArgumentNullException();
                return EntityUpdateHandler.Update<Sprint>(sprint.ToModel()).ToHttpResponse();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) throw new ArgumentNullException();
                return Ok(_sprintRepository.Delete(id));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromQuery] string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) throw new ArgumentNullException();
                return Ok(SprintDTO.FromModel(_sprintRepository.GetById(id)));
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
                return Ok(_sprintRepository.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
