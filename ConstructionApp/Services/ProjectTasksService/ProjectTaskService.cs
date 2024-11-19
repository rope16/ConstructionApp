using ConstructionApp.Dtos.Project;
using ConstructionApp.Dtos.ProjectTask;
using ConstructionApp.Interfaces.ProjectTasksInterface;
using ConstructionApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionApp.Services.ProjectTasksService
{
    public class ProjectTaskService : IProjectTaskInterface
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger <ProjectTaskService> _logger;

        public ProjectTaskService(ApplicationDbContext context, ILogger<ProjectTaskService> logger )
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ProjectTaskDetailsDto> CreateProjectTask(ProjectTaskCreateDto dto)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == dto.ProjectId);

            string base64Image = "";

            if (project is null)
            {
                _logger.LogInformation("Project with provided ID wasn't found");
                return new ProjectTaskDetailsDto();
            }

            //if (image != null && image.Length != 0)
            //{
            //    using (var memoryStream = new MemoryStream())
            //    {
            //        await image.CopyToAsync(memoryStream);
            //        byte[] imageBytes = memoryStream.ToArray();
            //        base64Image = Convert.ToBase64String(imageBytes);
            //    }
            //}

            var newProjectTask = ProjectTask.CreateProjectTask(dto.Note, dto.StartDate, dto.EndDate, dto.ProjectId);

            var response = new ProjectTaskDetailsDto
            {
                ProjectTaskId = newProjectTask.ProjectTaskId,
                Note = newProjectTask.Note,
                Status = newProjectTask.Status,
                StartDate = newProjectTask.StartDate,
                EndDate = newProjectTask.EndDate,
                Project = new ProjectDetailsDto
                {
                    ProjectId = project.ProjectId,
                    Note = project.Note,
                    Name = project.Name,
                    Status = project.Status,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    ConstructionSiteId = project.ConstructionSiteId
                }
            };

            _context.Add(newProjectTask);

            await _context.SaveChangesAsync();

            return response;
        }
    }
}
