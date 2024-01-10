using SolarWatch.Model;

namespace SolarWatch.Service.Processors;

public interface ISolarJsonProcessor
{
    SolarData? Process(string data, DateTime date);
}