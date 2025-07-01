using SearchService.API.DTO;
using SearchService.BL.DomainModels;

namespace SearchService.API.Mappers;

public static class SearchMapper
{
    public static SearchRequest ToDomain(SearchRequestDto dto)
        => new(
            dto.ServiceName,
            dto.UserLocation.Lat,
            dto.UserLocation.Lng
        );

    public static SearchResponseDto ToDto(SearchResponse domain)
        => new(
            domain.TotalHits,
            domain.TotalDocuments,
            domain.Results.Select(r => new SearchResultDto(
                r.Id,
                r.Name,
                new GeoPositionDto(r.Lat, r.Lng),
                r.Distance,
                r.Score
            )).ToList()
        );
}