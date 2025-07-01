using Microsoft.Extensions.Logging;
using SearchService.Infra.Entities;
using SearchService.Infra.Interfaces;
using System.Text.Json;

namespace SearchService.Infra.Repositories
{
    public class JsonServiceRepository : IServiceRepository
    {
        private readonly string _filePath;
        private readonly ILogger<JsonServiceRepository> _logger;

        public JsonServiceRepository(string filePath, ILogger<JsonServiceRepository> logger)
        {
            _filePath = filePath;
            _logger = logger;
        }

        public async Task<IEnumerable<ServiceEntity>> GetAllAsync()
        {
            if (!File.Exists(_filePath))
            {
                _logger.LogError("Service data file not found: {FilePath}", _filePath);
                throw new FileNotFoundException($"Service data file not found: {_filePath}");
            }

            try
            {
                var json = await File.ReadAllTextAsync(_filePath);
                var data = JsonSerializer.Deserialize<List<ServiceEntity>>(json);

                var filtered = data?
                    .Where(e => e != null && e.Id != 0 && !string.IsNullOrWhiteSpace(e.Name) && e.Position != null)
                    .ToList() ?? new List<ServiceEntity>();

                _logger.LogInformation("Loaded {Count} valid services from {FilePath}", filtered.Count, _filePath);
                return filtered;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to read or deserialize service data from {FilePath}", _filePath);
                throw;
            }
        }
    }
}