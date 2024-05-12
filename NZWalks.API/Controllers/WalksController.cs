using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // /api /walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;
        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
        }
        //CREATE WALK
        //POST: /api/walks

        [HttpPost]

        public async Task<IActionResult> CreateWalkAsync([FromBody] AddWalkRequestDTO addWalkRequestDTO) {

            //Map DTO to Domain
            var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDTO);

            //Create Walk
            await _walkRepository.CreateWalkAsync(walkDomainModel);

            //Map Domain Model to DTO

            var walkDTO = _mapper.Map<WalkDTO>(walkDomainModel);

            return Ok(walkDTO);
        }

        //GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAllWalk()
        {
            var walkDomainModel = await _walkRepository.GetAllWalkAsync();

            //Map Domain Model to DTO
            var walkDTO = _mapper.Map<List<WalkDTO>>(walkDomainModel);

            return Ok(walkDTO);
        }
    }
}
