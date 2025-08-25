using DevFreela.Application.Models;
using DevFreela.Application.Models.InputModels;
using DevFreela.Application.Models.ViewModels;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.Application.Services
{
    public interface IProjectService
    {
        ResultViewModel<List<ProjectItemViewModel>> GetAll(string? seach = "");
        ResultViewModel<ProjectViewModel> GetById(int id);
        ResultViewModel<int> Insert(CreateProjectInputModel model);
        ResultViewModel Update(UpdateProjectInputModel model);
        ResultViewModel Delete(int id);
        ResultViewModel Start(int id);
        ResultViewModel Complete(int id);
        ResultViewModel AddComment(CreateProjectCommentInputModel model);

        public class ProjectService : IProjectService
        {
            private readonly DevFreelaDbContext _context;

            public ProjectService(DevFreelaDbContext context)
            {
                _context = context;
            }

            public ResultViewModel AddComment(CreateProjectCommentInputModel model)
            {
                var project = _context.Projects.SingleOrDefault(p => p.Id == model.IdProject);
                if (project == null)
                {
                    return ResultViewModel.Failure("Project not found.");
                }

                var comment = new ProjectComment(model.Content, model.IdProject, model.IdUser);

                _context.ProjectComments.Add(comment);
                _context.SaveChanges();

                return ResultViewModel.Success("Comment added!");
            }

            public ResultViewModel Delete(int id)
            {
                var project = _context.Projects.SingleOrDefault(p => p.Id == id);
                if (project == null)
                {
                    return ResultViewModel.Failure("Project not found.");
                }

                project.SetAsDeleted();
                _context.Projects.Update(project);
                _context.SaveChanges();

                return ResultViewModel.Success("Project deleted!");
            }

            public ResultViewModel Complete(int id)
            {
                var project = _context.Projects.SingleOrDefault(p => p.Id == id);
                if (project == null)
                {
                    return ResultViewModel.Failure("Project not found");
                }

                project.Complete();
                _context.Projects.Update(project);
                _context.SaveChanges();

                return ResultViewModel.Success("Project completed!");
            }

            public ResultViewModel<List<ProjectItemViewModel>> GetAll(string? seach = "")
            {
                var projects = _context.Projects
                .Include(p => p.Client)
                .Include(p => p.Freelancer)
                .Where(p => !p.IsDeleted).ToList();

                var model = projects.Select(ProjectItemViewModel.FromEntity).ToList();

                return ResultViewModel<List<ProjectItemViewModel>>.Success(model);
            }

            public ResultViewModel<ProjectViewModel> GetById(int id)
            {
                var project = _context.Projects
               .Include(p => p.Client)
               .Include(p => p.Freelancer)
               .Include(p => p.Comments)
               .SingleOrDefault(p => p.Id == id);

                if (project == null)
                {
                    return ResultViewModel<ProjectViewModel>.Failure("Project not found.");
                }

                var model = ProjectViewModel.FromEntity(project);
                return ResultViewModel<ProjectViewModel>.Success(model);
            }

            public ResultViewModel<int> Insert(CreateProjectInputModel model)
            {
                var project = model.ToEntity();
                _context.Projects.Add(project);
                _context.SaveChanges();

                return ResultViewModel<int>.Success(project.Id);
            }

            public ResultViewModel Start(int id)
            {
                var project = _context.Projects.SingleOrDefault(p => p.Id == id);
                if (project == null)
                {
                    return ResultViewModel.Failure("Project not found");
                }

                project.Start();
                _context.Projects.Update(project);
                _context.SaveChanges();

                return ResultViewModel.Success("Project started!");
            }

            public ResultViewModel Update(UpdateProjectInputModel model)
            {
                var project = _context.Projects.SingleOrDefault(p => p.Id == model.IdProject);
                if (project == null)
                {
                    return ResultViewModel.Failure("Project not found");
                }
               

                project.Update(model.Title, model.Description, model.TotalCost);

                _context.Projects.Update(project);
                _context.SaveChanges();

                return ResultViewModel.Success("Project updated!");
            }
        }

    }
}
