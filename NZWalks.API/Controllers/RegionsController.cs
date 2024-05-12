using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _nZWalksDbContext;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(NZWalksDbContext nZWalksDbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            _nZWalksDbContext = nZWalksDbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        // GET: api/regions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegionDTO>>> GetAll()
        {
            // Get all regions from the database asynchronously
            //var regionsDomain = await _nZWalksDbContext.Regions.ToListAsync(); the old
            var regionsDomain = await _regionRepository.GetAllRegionAsync();


            //Map Domain Models to DTOs
            //var regionsDTO = new List<RegionDTO>();
            //foreach (var regionDomain in regionsDomain)
            //{
            //    regionsDTO.Add(new RegionDTO()
            //    {
            //        Id = regionDomain.Id,
            //        Name = regionDomain.Name,
            //        Code = regionDomain.Code,
            //        RegionImageUrl = regionDomain.RegionImageUrl,
            //    });
            //}

            //Map Domain Models to DTOs
            var regionsDTO = _mapper.Map<List<RegionDTO>>(regionsDomain);

            // Return DTOs
            return Ok(regionsDTO);
        }

        // GET: api/regions/{id}
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<RegionDTO>> GetRegionById(Guid id)
        {
            // Get region by id from the database asynchronously
            //var regionDomain = await _nZWalksDbContext.Regions.FirstOrDefaultAsync(u => u.Id == id);

            var regionDomain = await _regionRepository.GetRegionByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            // Map region domain model to DTO
            //var regionDTO = new RegionDTO
            //{
            //    Id = regionDomain.Id,
            //    Name = regionDomain.Name,
            //    Code = regionDomain.Code,
            //    RegionImageUrl = regionDomain.RegionImageUrl
            //};

            var regionDTO = _mapper.Map<RegionDTO>(regionDomain);

            // Return DTO back to client
            return Ok(regionDTO);
        }

        // POST: api/regions
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<RegionDTO>> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            // Map or convert DTO to domain model
            //var regionDomainModel = new Region
            //{
            //    Code = addRegionRequestDTO.Code,
            //    Name = addRegionRequestDTO.Name,
            //    RegionImageUrl = addRegionRequestDTO.RegionImageUrl
            //};

            
                var regionDomainModel = _mapper.Map<Region>(addRegionRequestDTO);

                // Use domain model to create region
                //_nZWalksDbContext.Regions.Add(regionDomainModel);
                //await _nZWalksDbContext.SaveChangesAsync();
                regionDomainModel = await _regionRepository.CreateRegionAsync(regionDomainModel);

                // Map domain model back to DTO
                //var regionDTO = new RegionDTO
                //{
                //    Id = regionDomainModel.Id,
                //    Name = regionDomainModel.Name,
                //    Code = regionDomainModel.Code,
                //    RegionImageUrl = regionDomainModel.RegionImageUrl
                //};

                var regionDTO = _mapper.Map<RegionDTO>(regionDomainModel);

                // Return created region DTO
                return CreatedAtAction(nameof(GetRegionById), new { id = regionDTO.Id }, regionDTO);
            }
      
        // PUT: api/regions/{id}
        [HttpPut("{id:Guid}")]
        [ValidateModel]
        public async Task<ActionResult<RegionDTO>> UpdateRegion(Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
           
                //Map DTO to Domain Model
                //var regionDomainModel = new Region
                //{
                //    Name = updateRegionRequestDTO.Name,
                //    Code = updateRegionRequestDTO.Code,
                //    RegionImageUrl = updateRegionRequestDTO.RegionImageUrl
                //};

                var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDTO);

                regionDomainModel = await _regionRepository.UpdateRegionAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                //// Map DTO to domain model
                //regionDomainModel.Code = updateRegionRequestDTO.Code;
                //regionDomainModel.Name = updateRegionRequestDTO.Name;
                //regionDomainModel.RegionImageUrl = updateRegionRequestDTO.RegionImageUrl;

                //await _nZWalksDbContext.SaveChangesAsync();

                // Convert domain model to DTO
                //var regionDTO = new RegionDTO
                //{
                //    Id = regionDomainModel.Id,
                //    Name = regionDomainModel.Name,
                //    Code = regionDomainModel.Code,
                //    RegionImageUrl = regionDomainModel.RegionImageUrl
                //};

                var regionDTO = _mapper.Map<RegionDTO>(regionDomainModel);

                // Return updated region DTO
                return Ok(regionDTO);           
        }

        // DELETE: api/regions/{id}
        [HttpDelete("{id:Guid}")]

        public async Task<ActionResult<RegionDTO>> DeleteRegion(Guid id)
        {
            //var regionDomainModel = await _nZWalksDbContext.Regions.FirstOrDefaultAsync(u => u.Id == id);
            var regionDomainModel = await _regionRepository.DeleteRegionAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Delete region
            //_nZWalksDbContext.Regions.Remove(regionDomainModel);
            //await _nZWalksDbContext.SaveChangesAsync();

            // Return deleted region DTO
            //var regionDTO = new RegionDTO
            //{
            //    Id = regionDomainModel.Id,
            //    Name = regionDomainModel.Name,
            //    Code = regionDomainModel.Code,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            var regionDTO = _mapper.Map<RegionDTO>(regionDomainModel);

            return Ok(regionDTO);
        }
    }
}
