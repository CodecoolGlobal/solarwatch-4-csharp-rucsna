using SolarWatch.Model;

namespace SolarWatch.Service.Processors;

public interface ICityJsonProcessor
{
    City Process(string data);
}