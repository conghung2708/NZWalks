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

        public async Task<IActionResult> CreateWalkAsync([FromBody] AddWalkRequestDTO addWalkRequestDTO)
        {
            if(ModelState.IsValid)
            {
                //Map DTO to Domain
                var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDTO);

                //Create Walk
                await _walkRepository.CreateWalkAsync(walkDomainModel);

                //Map Domain Model to DTO

                var walkDTO = _mapper.Map<WalkDTO>(walkDomainModel);

                return Ok(walkDTO);
            }
            else
            {
                return BadRequest(ModelState);
            }
          
        }

        //GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAllWalkAsync()
        {
            var walkDomainModel = await _walkRepository.GetAllWalkAsync();

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
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] UpdateWalkRequestDTO updateWalkRequestDTO)
        {
            if(ModelState.IsValid)
            {
                var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDTO);

                walkDomainModel = await _walkRepository.UpdateWalkAsync(id, walkDomainModel);

                var walkDTO = _mapper.Map<WalkDTO>(walkDomainModel);

                return Ok(walkDTO);
            }
            else
            {
                return BadRequest(ModelState);
            }
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
