namespace SearchService.BL.DomainModels;

public record SearchResponse(
    int TotalHits,
    int TotalDocuments,
    List<SearchResult> Results
);

public record SearchResult(
    int Id,
    string Name,
    double Lat,
    double Lng,
    string Distance,
    double Score
);