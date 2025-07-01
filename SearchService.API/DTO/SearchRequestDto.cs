namespace SearchService.API.DTO;

public record SearchRequestDto(
    string ServiceName,
    GeoPositionDto UserLocation
);