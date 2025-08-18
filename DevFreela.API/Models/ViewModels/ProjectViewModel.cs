using DevFreela.API.Entities;

namespace DevFreela.API.Models.ViewModels
{
    public class ProjectViewModel
    {
        public ProjectViewModel(int id, string title, string description, int idClient, string clientName, int idFreelancer, string freelancerName, decimal totalCost, DateTime? startedAt, DateTime? completedAt, List<ProjectComment> comments)
        {
            Id = id;
            Title = title;
            Description = description;
            IdClient = idClient;
            ClientName = clientName;
            IdFreelancer = idFreelancer;
            FreelancerName = freelancerName;
            TotalCost = totalCost;
            StartedAt = startedAt;
            CompletedAt = completedAt;  

            Comments = comments.Select(c => c.Content).ToList();
        }
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; } 
        public int IdClient { get; private set; }   
        public string ClientName { get; private set; }
        public int IdFreelancer { get; private set; }
        public string FreelancerName { get; private set; }
        public decimal TotalCost { get; private set; }
        public DateTime? StartedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public List<string> Comments { get; private set; }

        public static ProjectViewModel FromEntity(Project project)
            => new ProjectViewModel(
                project.Id,
                project.Title,
                project.Description,
                project.IdClient,
                project.Client.FullName,
                project.IdFreelancer,
                project.Freelancer?.FullName ?? "Not assigned",
                project.TotalCost,
                project.StartedAt,
                project.CompletedAt,
                project.Comments
            );
    }
}
