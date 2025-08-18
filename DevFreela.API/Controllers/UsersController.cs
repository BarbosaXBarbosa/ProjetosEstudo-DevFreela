using DevFreela.API.Models.InputModels;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post(CreateUserInputModel model)
        {
            return Ok();
        }

        [HttpPut("{id}/skills")]
        public IActionResult PostSkills(UserSkillsInputModel model)
        {
            return Ok();
        }

        [HttpPut("{id}/profile-picture")]

        public IActionResult PostProfilePicture(IFormFile file)
        {
            var description = $"File: {file.FileName}, Size: {file.Length}";

            // Processar imagem

            return Ok(description);
        }
    } 
}

