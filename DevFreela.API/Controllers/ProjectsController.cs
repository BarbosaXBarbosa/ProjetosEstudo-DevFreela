using DevFreela.API.Entities;
using DevFreela.API.Models.Config;
using DevFreela.API.Models.InputModels;
using DevFreela.API.Models.ViewModels;
using DevFreela.API.Persistence;
using DevFreela.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Get(string search="")
        {
            var projects = _context.Projects
                .Include(p => p.Client)
                .Include(p => p.Freelancer) 
                .Where(p => !p.IsDeleted).ToList();
            
            var model = projects.Select(ProjectItemViewModel.FromEntity).ToList();
            return Ok(model);
        }
        
        //GET api/projects/1234
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var project = _context.Projects
                .Include(p => p.Client)
                .Include(p => p.Freelancer)
                .Include(p => p.Comments)
                .SingleOrDefault(p => p.Id == id);
            
            if (project == null)
            {
                return NotFound();
            }
            
            var model = ProjectItemViewModel.FromEntity(project);
            
            return Ok(model);
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
            var project = model.ToEntity();
            _context.Projects.Add(project);
            _context.SaveChanges();
            
            
            return CreatedAtAction(nameof(GetById), new { id=1}, response);
        }

        // PUT api/projects/1234
        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateProjectInputModel model)
        {
            var project = _context.Projects.SingleOrDefault(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            
            var min = _configService.GetMinimumCost();
            var max = _configService.GetMaximumCost();
            if (model.TotalCost < min || model.TotalCost > max)
            {
                return BadRequest($"The total cost must be between {min} and {max}.");
            }
            
            project.Update(model.Title, model.Description, model.TotalCost);
            
            _context.Projects.Update(project);
            _context.SaveChanges();
            
            return NoContent();
        }
        
        // DELETE api/projects/1234
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var project = _context.Projects.SingleOrDefault(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            
            project.SetAsDeleted();
            _context.Projects.Update(project);
            _context.SaveChanges();
            
            return Ok();
        }
        
        // PUT api/projects/1234/start
        [HttpPut("{id}/start")]
        public IActionResult Start(int id)
        {
            var project = _context.Projects.SingleOrDefault(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            
            project.Start();
            _context.Projects.Update(project);
            _context.SaveChanges();
            
            return NoContent();
        }
        
        // PUT api/projects/1234/complete
        [HttpPut("{id}/complete")]
        public IActionResult Complete(int id)
        {
            var project = _context.Projects.SingleOrDefault(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            
            project.Complete();
            _context.Projects.Update(project);
            _context.SaveChanges();
            
            return NoContent();
        } 
        
        // POST api/projects/1234/comments
        [HttpPost("{id}/comments")]
        public IActionResult PostComments(int id, CreateProjectCommentInputModel model)
        {
            var project = _context.Projects.SingleOrDefault(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            
            var comment = new ProjectComment(model.Content, model.IdProject, model.IdUser);
            
            _context.ProjectComments.Add(comment);
            _context.SaveChanges();
            
            return Ok();
        }
        
    }
    
}    
