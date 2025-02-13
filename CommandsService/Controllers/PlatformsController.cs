using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController(ICommandRepo _repo, IMapper _mapper) : ControllerBase
    {
        private readonly ICommandRepo repo = _repo;
        private readonly IMapper mapper = _mapper;

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting platforms from CommandsService");
            
            var list = repo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(list));
        }

        [HttpPost]
        public ActionResult TestIndoundConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service");

            return Ok("Inbound test OK from Platforms Controller");
        }


    }
}