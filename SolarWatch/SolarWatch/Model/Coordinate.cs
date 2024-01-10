namespace SolarWatch.Model;

public class Coordinate
{
    public Guid CoordinateId { get; set; }
    public float Latitude { get; init; }
    public float Longitude { get; init; }

    public Coordinate()
    {
    }
}