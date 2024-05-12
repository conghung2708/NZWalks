using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
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
        [ValidateModel]
        public async Task<IActionResult> CreateWalkAsync([FromBody] AddWalkRequestDTO addWalkRequestDTO)
        {
                //Map DTO to Domain
                var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDTO);

                //Create Walk
                await _walkRepository.CreateWalkAsync(walkDomainModel);

                //Map Domain Model to DTO

                var walkDTO = _mapper.Map<WalkDTO>(walkDomainModel);

                return Ok(walkDTO);
            }

        //GET ALL
        //GET: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending&pageNumber=1&pageSize=10 => find Walk that contains Track
        [HttpGet]
        public async Task<IActionResult> GetAllWalkAsync([FromQuery]string? filterOn,[FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walkDomainModel = await _walkRepository.GetAllWalkAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            //Map Domain Model to DTO
            var walkDTO = _mapper.Map<List<WalkDTO>>(walkDomainModel);

            return Ok(walkDTO);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetWalkByIdAsync([FromRoute] Guid id)
        {
            var walkDomainModel = await _walkRepository.GetWalkByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            var walkDTO = _mapper.Map<WalkDTO>(walkDomainModel);
            return Ok(walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] UpdateWalkRequestDTO updateWalkRequestDTO)
        {
                var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDTO);

                walkDomainModel = await _walkRepository.UpdateWalkAsync(id, walkDomainModel);

                var walkDTO = _mapper.Map<WalkDTO>(walkDomainModel);

                return Ok(walkDTO);
   
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteWalkAsync([FromRoute] Guid id)
        {
            var walkDomainModel = await _walkRepository.DeleteAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            var walkDTO = _mapper.Map<WalkDTO>(walkDomainModel);

            return Ok(walkDTO);
        }

    }
}
