using SolarWatch.Model;

namespace SolarWatch.Processors;

public interface ISolarJsonProcessor
{
    SolarData Process(string data, string city, DateTime date);
}