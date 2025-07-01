using Microsoft.Extensions.Logging;
using SearchService.BL.DomainModels;
using SearchService.BL.Helpers;
using SearchService.BL.Interfaces;
using SearchService.BL.Mappers;
using SearchService.Infra.Interfaces;

namespace SearchService.BL;

public class SearchApplicationService : ISearchService
{
    private readonly IServiceRepository _repo;
    private readonly ILogger<SearchApplicationService> _logger;

    public SearchApplicationService(IServiceRepository repo, ILogger<SearchApplicationService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<SearchResponse> SearchAsync(SearchRequest query)
    {
        _logger.LogInformation("Starting search for: {ServiceName} at ({Lat}, {Lng})", query.ServiceName, query.Lat, query.Lng);

        var all = (await _repo.GetAllAsync())
            .Select(ServiceMapper.ToDomain)
            .ToList();

        _logger.LogDebug("Fetched {Count} services from repository.", all.Count());

        var results = all
            .Select(service =>
            {
                var score = ScoreMatch(query.ServiceName, service.Name);
                var dist = GeoHelper.CalculateDistanceKm(query.Lat, query.Lng, service.Position.Lat, service.Position.Lng);
                return new { service, score, dist };
            })
            .Where(x => x.score > 0)
            .OrderByDescending(x => x.score)
            .ThenBy(x => x.dist)
            .Select(x => new SearchResult(
                x.service.Id,
                x.service.Name,
                x.service.Position.Lat,
                x.service.Position.Lng,
                $"{x.dist:F2}km",
                Math.Round(x.score, 2)
            ))
            .ToList();

        _logger.LogInformation("Search completed. Returning {Count} results.", results.Count);

        return new SearchResponse(
            TotalHits: results.Count,
            TotalDocuments: all.Count(),
            Results: results
        );
    }

    private static double ScoreMatch(string input, string target)
    {
        var i = input.Trim().ToLowerInvariant();
        var t = target.Trim().ToLowerInvariant();

        if (i == t) return 1.0;
        if (t.StartsWith(i)) return 0.9;
        if (t.Contains(i)) return 0.75;
        return 0.0;
    }
}