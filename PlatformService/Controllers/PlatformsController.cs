using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Repositories;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class PlatformsController(IPlatformRepository platformRepository, 
        IMapper mapper, ICommandDataClient dataClient) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly ICommandDataClient _dataClient = dataClient;
        private readonly IPlatformRepository _platformRepository = platformRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetPlatforms()
        {
            try
            {
                var res = await _platformRepository.GetAllPlatforms();
            
                return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(res));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public async Task<ActionResult<PlatformReadDto>> GetPlatformById(int id)
        {
            try
            {
                var res = await _platformRepository.GetPlatformById(id);
                return Ok(_mapper.Map<PlatformReadDto>(res));
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
            catch(Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform([FromBody] PlatformCreateDto dto)
        {
            try
            {
                var platform = _mapper.Map<Platform>(dto);  

                await _platformRepository.AddPlatform(platform);

                var res = _mapper.Map<PlatformReadDto>(platform);

                try
                {
                    await _dataClient.SendPlatformToCommand(res);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"--> Could not send synchronously: {e.Message}");
                }

                return CreatedAtRoute(nameof(GetPlatformById), new { Id = res.Id }, res);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}