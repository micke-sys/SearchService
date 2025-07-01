namespace SearchService.BL.DomainModels;

public record SearchRequest(
    string ServiceName,
    double Lat,
    double Lng
);