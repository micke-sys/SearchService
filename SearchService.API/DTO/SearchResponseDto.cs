namespace SearchService.API.DTO;

public record SearchResponseDto(
    int TotalHits,
    int TotalDocuments,
    List<SearchResultDto> Results
);

public record SearchResultDto(
    int Id,
    string Name,
    GeoPositionDto Position,
    string Distance,
    double Score
);