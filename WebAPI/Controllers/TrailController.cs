using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data.Repository.IRepository;
using WebAPI.Models;
using WebAPI.Models.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/trail")]
    [ApiController]
    //[Authorize]
    public class TrailController : ControllerBase
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;
        public TrailController(ITrailRepository trailRepository, IMapper mapper)
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetTrails()
        {
            return Ok(_trailRepository.GetTrails().Select(_mapper.Map<Trails,TrailsDto>));
        }
        [HttpGet("{trailId:int}",Name = "GetTrail")]
        public IActionResult GetTrail(int trailId)
        {
            var trail=_trailRepository.GetTrail(trailId);
            if (trail == null) return BadRequest();
            var traildto = _mapper.Map<TrailsDto>(trail);
            return Ok(traildto);
        }
        [HttpPost]
        public IActionResult CreateTrail([FromBody]TrailsDto trailsDto)
        {
            if(trailsDto==null) return BadRequest();
            if (_trailRepository.TrailsExists(trailsDto.Id))
            {
                ModelState.AddModelError("", $"Tral already in DB");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!ModelState.IsValid) return BadRequest();
            var trails = _mapper.Map<Trails>(trailsDto);
            if (!_trailRepository.CreateTrails(trails))
            {
                ModelState.AddModelError("", $"Something Went Wrong While Creating Data : {trails.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtRoute("GetTrail", new { trailId = trails.Id }, trails);

        }
        [HttpPut]
        public IActionResult UpdateTrail([FromBody]TrailsDto trailsDto)
        {
            if (trailsDto == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var trail = _mapper.Map<Trails>(trailsDto);
            if (!_trailRepository.UpdateTrails(trail))
            {
                ModelState.AddModelError("", $"Something Went Wrong While Updating Data : {trail.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }
        [HttpDelete("{trailId:int}")]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_trailRepository.TrailsExists(trailId)) return BadRequest();
            var trail=_trailRepository.GetTrail(trailId);
            if (trail == null) return BadRequest();
            if (!_trailRepository.DeleteTrails(trail))
            {
                ModelState.AddModelError("", $"Something Went Wrong While Deleting Data : {trail.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}
