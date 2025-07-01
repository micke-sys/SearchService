using Microsoft.AspNetCore.Mvc;
using SearchService.API.DTO;
using SearchService.API.Mappers;
using SearchService.BL.Interfaces;

namespace SearchService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;
    private readonly ILogger<SearchController> _logger;

    public SearchController(ISearchService searchService, ILogger<SearchController> logger)
    {
        _searchService = searchService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(SearchResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SearchResponseDto>> Search([FromBody] SearchRequestDto request)
    {
        _logger.LogInformation("Received search request: {@Request}", request);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Validation failed: {@ModelState}", ModelState);
            return BadRequest(ModelState);
        }

        try
        {
            var domainRequest = SearchMapper.ToDomain(request);
            _logger.LogDebug("Mapped to domain request: {@DomainRequest}", domainRequest);

            var domainResponse = await _searchService.SearchAsync(domainRequest);
            _logger.LogInformation("Search completed. Hits: {TotalHits}", domainResponse.TotalHits);

            var responseDto = SearchMapper.ToDto(domainResponse);

            return Ok(responseDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while processing search request.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}