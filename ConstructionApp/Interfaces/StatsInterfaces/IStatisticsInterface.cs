using ConstructionApp.Dtos.Stats;

namespace ConstructionApp.Interfaces.StatsInterfaces
{
    public interface IStatisticsInterface
    {
        Task<StatsDto> GetGeneralStatistics();
    }
}
