namespace SolarWatch.Model;

public class SolarData
{
    public Guid SolarDataId { get; set; }
    public Guid CityId { get; set; }
    public DateTime Date { get; set; }
    public string Sunrise { get; set; }
    public string Sunset { get; set; }

    public SolarData()
    {
    }
}