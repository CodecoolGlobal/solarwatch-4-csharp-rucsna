using SolarWatch.Model;

namespace SolarWatch.Processors;

public interface ICityJsonProcessor
{
    CityData Process(string data);
}