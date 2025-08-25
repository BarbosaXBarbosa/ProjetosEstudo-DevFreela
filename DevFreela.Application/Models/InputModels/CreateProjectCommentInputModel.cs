using DevFreela.Core.Entities;
using System.Net.Sockets;

namespace DevFreela.Application.Models.InputModels;

public class CreateProjectCommentInputModel
{
    public string Content { get; set; }
    public int IdUser { get; set; }
    public int IdProject { get; set; }
}