using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var regionsDomain = dbContext.Region.ToList();

            var regionsDto = new List<RegionDto>();
            foreach (var region in regionsDomain) {
                regionsDto.Add(new RegionDto() 
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code    = region.Code,
                    RegionImageUrl  = region.RegionImageUrl
                });
            }

            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
          // var region = dbContext.Region.Find(id);
          var regionsDomain = dbContext.Region.FirstOrDefault(x => x.Id == id);
            if (regionsDomain == null) {
                return NotFound();
            }
            var regionsDto = new RegionDto()
            {
                Id = regionsDomain.Id,
                Name = regionsDomain.Name,
                Code = regionsDomain.Code,
                RegionImageUrl = regionsDomain.RegionImageUrl
            };
            return Ok(regionsDomain);
        }
        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            dbContext.Region.Add(regionDomainModel);
            dbContext.SaveChanges();

            var regionsDto = new RegionDto
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            }; 
            dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionsDto);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomainModel = dbContext.Region.FirstOrDefault(s => s.Id == id);
            if(regionDomainModel == null)
            {
                return NotFound();  
            }
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name    = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            dbContext.SaveChanges();

            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            // check if region with the given id exists in database
            var regionDomainModel = dbContext.Region.FirstOrDefault(x => x.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }
            dbContext.Region.Remove(regionDomainModel);
            dbContext.SaveChanges();

            var regindto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regindto);
        }
    }
}
