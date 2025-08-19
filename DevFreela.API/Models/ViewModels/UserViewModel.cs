using DevFreela.API.Entities;

namespace DevFreela.API.Models.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel(string fullName, string email, DateTime birthDate, List<string> skills)
        {
            FullName = fullName;
            Email = email;
            BirthDate = birthDate;
            Skills = skills ?? new List<string>();
        }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public List<string> Skills { get; set; }

        public static UserViewModel FromEntity(User user)
            => new UserViewModel(
                user.FullName,
                user.Email,
                user.BirthDate,
                user.Skills.Select(us => us.Skill.Description).ToList()
            );
    }
}
