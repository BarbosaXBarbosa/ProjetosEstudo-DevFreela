using DevFreela.API.Models.InputModels;
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
            var skills = _context.UserSkills
                .ToList();
            return Ok();
        }

        [HttpPost]

        public IActionResult Post(CreateSkillInputModel model)
        {
            return Ok();
        }

    } 
}
