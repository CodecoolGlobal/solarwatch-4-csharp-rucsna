using SolarWatch.Model;

namespace SolarWatch.Service.Repository;

public interface ISolarWatchRepository
{
    Task<SolarData?> GetDataAndAddToDbAsync(string cityName, DateTime date);
}