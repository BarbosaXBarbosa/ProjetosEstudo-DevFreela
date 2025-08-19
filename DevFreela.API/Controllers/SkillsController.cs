using DevFreela.API.Models.InputModels;
using DevFreela.API.Models.ViewModels;
using DevFreela.API.Persistence;
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
            var skills = _context.Skills
               .Select(s => new SkillViewModel(
                   s.Id,
                   s.Description,
                   s.UserSkills.Count 
               ))
               .OrderBy(s => s.Description)
               .ToList();

            return Ok(skills);
        }

        [HttpPost]

        public IActionResult Post(CreateSkillInputModel model)
        {
            var skill = new Entities.Skill(model.Description);

            _context.Skills.Add(skill);
            _context.SaveChanges();

            return NoContent();
        }

    } 
}
