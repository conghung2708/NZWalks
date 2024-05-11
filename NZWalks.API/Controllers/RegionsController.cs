using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    //https://localhost:1234/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _nZWalksDbContext;
        public RegionsController(NZWalksDbContext nZWalksDbContext)
        {
            _nZWalksDbContext = nZWalksDbContext;
        }
        //GET ALL REGIONS
        //GET: https://localhost:portnumber/api/regions
        [HttpGet]
        public IActionResult GetAll()
        {
            //Get Data from Database - Domain Models
            var regionsDomain = _nZWalksDbContext.Regions.ToList();
            //Map Domain Models to DTOs
            var regionsDTO = new List<RegionDTO>();
            foreach (var regionDomain in regionsDomain)
            {
                regionsDTO.Add(new RegionDTO()
                {
                    Id = regionDomain.Id,
                    Name = regionDomain.Name,
                    Code = regionDomain.Code,
                    RegionImageUrl = regionDomain.RegionImageUrl,
                });
            }

            //return DTOs
            return Ok(regionsDTO);
        }

        //GET REGION BY ID
        //GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetRegionById([FromRoute] Guid id) {

            //Get Region Domain Model From Database
            var regionDomain = _nZWalksDbContext.Regions.FirstOrDefault(u => u.Id == id);

            //var region2 = _nZWalksDbContext.Regions.Find(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            //Map Region Domain Model to DTO
            var regionDTO = new RegionDTO
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            //Return DTO back to Client
            return Ok(regionDTO);
        }
    }
}
