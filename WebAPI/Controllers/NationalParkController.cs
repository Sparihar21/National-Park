using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data.Repository.IRepository;
using WebAPI.Models;
using WebAPI.Models.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/NationalPark")]
    [ApiController]
    //[Authorize]
    public class NationalParkController : ControllerBase
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;
        public NationalParkController(INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var NationalParkDto = _nationalParkRepository.GetNationalParks().Select(_mapper.Map<NationalPark, NationalParkDto>);
            return Ok(NationalParkDto);  //200
        }
        [HttpGet("{NationalParkId:int}",Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int NationalParkId)
        {
            var nationalPark=_nationalParkRepository.GetNationalPark(NationalParkId);
            if (nationalPark == null) return NotFound();
            var NationalParkDto = _mapper.Map<NationalParkDto>(nationalPark);
            return Ok(NationalParkDto);
        }
        [HttpPost]
        public IActionResult CreateNationalPark([FromBody]NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null) return NotFound();
            if (_nationalParkRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park in DB");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!ModelState.IsValid) return BadRequest();
            var nationalPark = _mapper.Map<NationalParkDto, NationalPark>(nationalParkDto);
            if (!_nationalParkRepository.CreateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong while save data:{nationalPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            //return Ok();
            return CreatedAtRoute("GetNationalPark", new { NationalParkId = nationalPark.Id }, nationalPark);
        }
        [HttpPut]
        public IActionResult UpdateNationalPark([FromBody]NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null) return BadRequest();
            if (!ModelState.IsValid) return NotFound();
            var nationalPark = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_nationalParkRepository.UpdateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong while updating data:{nationalPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }
        [HttpDelete("{nationalParkId:int}")]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_nationalParkRepository.NationalParkExists(nationalParkId)) return NotFound();
            var nationalPark = _nationalParkRepository.GetNationalPark(nationalParkId);
            if (nationalPark == null) return NotFound();
            if (!_nationalParkRepository.DeleteNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong while deleting data:{nationalPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}
