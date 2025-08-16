namespace DevFreela.API.Models.InputModels;

public class CreateProjectCommentInputModel
{
    public string Content { get; set; }
    public int IdUser { get; set; }
    public int IdProject { get; set; }
}