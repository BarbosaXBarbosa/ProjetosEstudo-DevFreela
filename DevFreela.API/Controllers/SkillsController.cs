using DevFreela.Application.Models.InputModels;
using DevFreela.Application.Models.ViewModels;
using DevFreela.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers
{
    [ApiController]
    [Route("api/skills")]
    public class SkillsController : ControllerBase
    {
        private readonly DevFreelaDbContext _context;
        public SkillsController(DevFreelaDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var skills = _context.Skills.ToList();
            var model = skills.Select(SkillViewModel.FromEntity).ToList();

            return Ok(model);
        }

        [HttpPost]

        public IActionResult Post(CreateSkillInputModel model)
        {
            var skill = new Core.Entities.Skill(model.Description);

            _context.Skills.Add(skill);
            _context.SaveChanges();

            return NoContent();
        }

    } 
}
