namespace DevFreela.API.Models.ViewModels
{
    public class SkillViewModel
    {
        public SkillViewModel(int id, string description, int usersCount)
        {
            Id = id;
            Description = description;
            UsersCount = usersCount;
        }

        public int Id { get; private set; }
        public string Description { get; private set; }
        public int UsersCount { get; private set; }
        
        public static SkillViewModel FromEntity(Entities.Skill skill)
        {
            return new SkillViewModel(
                skill.Id,
                skill.Description,
                skill.UserSkills?.Count ?? 0
            );
        }
    }
}