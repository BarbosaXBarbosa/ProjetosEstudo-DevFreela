using DevFreela.API.Entities;

namespace DevFreela.API.Models.InputModels;
public class CreateProjectInputModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int IdCLient {  get; set; }
    public int IdFreelancer {  get; set; }
    public decimal TotalCost { get; set; }

    public Project ToEntity()
        => new(
            Title,
            Description,
            IdCLient,
            IdFreelancer,
            TotalCost
        );
}