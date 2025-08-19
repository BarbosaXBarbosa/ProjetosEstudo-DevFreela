using DevFreela.API.Entities;
using DevFreela.API.Models.InputModels;
using DevFreela.API.Models.ViewModels;
using DevFreela.API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {

        private readonly DevFreelaDbContext _context;
        public UsersController(DevFreelaDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id) {
            var user = _context.Users
                .Include(u => u.Comments)
                .Include(u => u.Skills)
                    .ThenInclude(us => us.Skill)
                .SingleOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            var model = UserViewModel.FromEntity(user);
            return Ok(model);
        }
        [HttpPost]
        public IActionResult Post(CreateUserInputModel model)
        {
            var user = new User(model.FullName, model.Email, model.BirthDate, model.Password);

            _context.Users.Add(user);
            _context.SaveChanges();


            return Ok();
        }

        [HttpPut("{id}/skills")]
        public IActionResult UpdateSkills(int id, UpdateUserSkillInputModel model)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            // Obter skills atuais do usuário
            var currentUserSkills = _context.UserSkills
                .Where(us => us.IdUser == id)
                .ToList();

            // Identificar skills para remover (as que não estão na nova lista)
            var newSkillIds = model.SkillIds ?? Array.Empty<int>();
            var skillsToRemove = currentUserSkills
                .Where(us => !newSkillIds.Contains(us.IdSkill))
                .ToList();

            // Identificar novas skills para adicionar
            var existingSkillIds = currentUserSkills.Select(us => us.IdSkill);
            var skillsToAdd = newSkillIds
                .Except(existingSkillIds)
                .Select(skillId => new UserSkill(id, skillId))
                .ToList();

            // Aplicar mudanças
            _context.UserSkills.RemoveRange(skillsToRemove);
            _context.UserSkills.AddRange(skillsToAdd);

            _context.SaveChanges();

            return NoContent(); 
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

