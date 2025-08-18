using DevFreela.API.Models.Config;
using DevFreela.API.Models.InputModels;
using DevFreela.API.Persistence;
using DevFreela.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DevFreela.API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly DevFreelaDbContext _context;
        private readonly IConfigService _configService;

        public ProjectsController(IConfigService configService, DevFreelaDbContext context)
        {
            _context = context;
            _configService = configService;
        }

        // GET api/projects?search=crm
        [HttpGet]
        public IActionResult Get(string search)
        {
            return Ok();
        }
        
        //GET api/projects/1234
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok();
        }
        
        // POST api/projects
        [HttpPost]
        public IActionResult Post(CreateProjectInputModel model)
        {
            var min = _configService.GetMinimumCost();
            var max = _configService.GetMaximumCost();
            if (model.TotalCost < min || model.TotalCost > max)
            {
                return BadRequest($"The total cost must be between {min} and {max}.");
            }

            var commissionPercentage = _configService.GetPlatformCommission(); // ex: 0.10
            var commissionValue = decimal.Round(model.TotalCost * commissionPercentage, 2);

            var expirationDays = _configService.GetProposalExpirationDays();

            var response = new
            {
                proposalExpiryDays = expirationDays,
                commissionPercentage = commissionPercentage,
                commissionValue = commissionValue
            };

            return CreatedAtAction(nameof(GetById), new { id=1}, response);
        }

        // PUT api/projects/1234
        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateProjectInputModel model)
        {
            return NoContent();
        }
        
        // DELETE api/projects/1234
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return NoContent();
        }
        
        // PUT api/projects/1234/start
        [HttpPut("{id}/start")]
        public IActionResult Start(int id)
        {
            return NoContent();
        }
        
        // PUT api/projects/1234/complete
        [HttpPut("{id}/complete")]
        public IActionResult Complete(int id)
        {
            return NoContent();
        } 
        
        // POST api/projects/1234/comments
        [HttpPost("{id}/comments")]
        public IActionResult PostComments(int id, CreateProjectCommentInputModel model)
        {
            return Ok();
        }
        
    }
    
}    
