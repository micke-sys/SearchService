using SearchService.BL.DomainModels;

namespace SearchService.BL.Interfaces;

public interface ISearchService
{
    Task<SearchResponse> SearchAsync(SearchRequest request);
}