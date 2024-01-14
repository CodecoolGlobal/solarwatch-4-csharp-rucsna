namespace SolarWatch.Model;

public class City
{
    public Guid CityId { get; set; }
    public string Name { get; set; }
    public Coordinate Coordinate { get; set; }
    public string? State { get; set; }
    public string Country { get; set; }

    public City()
    {
    }

    public void Update(City updateRequest)
    {
        Name = updateRequest.Name;
        Coordinate = updateRequest.Coordinate;
        State = updateRequest.State;
        Country = updateRequest.Country;
    }
}