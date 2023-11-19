namespace BookingsWebApi.Dtos;

public class CityDto
{
    public string CityId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
}

public class CityInsertDto
{
    public string Name { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
}
