using Microsoft.AspNetCore.Mvc;
using DevFreela.API.Models.InputModels;

namespace DevFreela.API.Controllers
{
    [ApiController]
    [Route("[api/projects]")]
    public class ProjectsController : ControllerBase
    {
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
            return CreatedAtAction(nameof(GetById), new { id=1}, model);
        }

        // PUT api/projects/1234
        [HttpPut("{ID}")]
        public IActionResult Put(int id, UpdateProjectInputModel model)
        {
            return NoContent();
        }
        
    }
    
}    
