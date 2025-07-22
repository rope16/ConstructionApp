using ConstructionApp.Dtos.Stats;
using ConstructionApp.Interfaces.StatsInterfaces;
using ConstructionApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionApp.Services.StatsServices
{
    public class StatisticsService : IStatisticsInterface
    {
        private readonly ApplicationDbContext _context;

        public StatisticsService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<StatsDto> GetGeneralStatistics()
        {
            var projectCount = await _context.Projects.CountAsync();

            var constructionSiteCount = await _context.ConstructionSites.CountAsync();

            var userCount = await _context.Users.CountAsync();

            var completedTasksCount = await _context.ProjectTasks
                .Where(pt => pt.Status == ProjectTaskStatus.Completed.ToString())
                .CountAsync();

            var inProgressTasksCount = await _context.ProjectTasks
                .Where(pt => pt.Status == ProjectTaskStatus.InProgress.ToString())
                .CountAsync();

            return new StatsDto
            {
                ProjectCount = projectCount,
                ConstructionSitesCount = constructionSiteCount,
                UsersCount = userCount,
                CompletedTasksCount = completedTasksCount,
                ActiveTasksCount = inProgressTasksCount
            };
        }
    }
}
