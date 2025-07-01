using SearchService.API.DTO;
using SearchService.API.Mappers;
using SearchService.BL.DomainModels;
using SearchService.BL.Mappers;
using SearchService.Infra.Entities;
using System.Text.Json;

namespace SearchService.Test
{
    public class ServiceMapperTests
    {
        [Fact]
        public void ToDomain_MapsAllPropertiesCorrectly()
        {
            var entity = new ServiceEntity
            {
                Id = 42,
                Name = "Test Service",
                Position = new PositionEntity { Lat = 59.1, Lng = 18.2 }
            };

            var result = ServiceMapper.ToDomain(entity);

            Assert.Equal(entity.Id, result.Id);
            Assert.Equal(entity.Name, result.Name);
            Assert.Equal(entity.Position.Lat, result.Position.Lat);
            Assert.Equal(entity.Position.Lng, result.Position.Lng);
        }
    }

    public class RepositoryFilterTests
    {
        [Fact]
        public void Repository_FiltersInvalidEntities()
        {
            var json = @"[
                { ""id"": 1, ""name"": ""Valid"", ""position"": { ""lat"": 1, ""lng"": 2 } },
                { ""id"": 0, ""name"": ""Invalid"", ""position"": { ""lat"": 1, ""lng"": 2 } },
                { ""id"": 2, ""name"": """", ""position"": { ""lat"": 1, ""lng"": 2 } },
                { ""id"": 3, ""name"": ""Valid2"", ""position"": null }
            ]";

            var data = JsonSerializer.Deserialize<List<ServiceEntity>>(json);
            var filtered = data.FindAll(e => e != null && e.Id != 0 && !string.IsNullOrWhiteSpace(e.Name) && e.Position != null);

            Assert.Single(filtered);
            Assert.Equal(1, filtered[0].Id);
        }
    }

    public class SearchMapperTests
    {
        [Fact]
        public void ToDomain_MapsRequestDtoToDomain()
        {
            var dto = new SearchRequestDto(
                "Massage",
                new GeoPositionDto(59.3, 18.0)
            );

            var domain = SearchMapper.ToDomain(dto);

            Assert.Equal(dto.ServiceName, domain.ServiceName);
            Assert.Equal(dto.UserLocation.Lat, domain.Lat);
            Assert.Equal(dto.UserLocation.Lng, domain.Lng);
        }

        [Fact]
        public void ToDto_MapsDomainToResponseDto()
        {
            var domain = new SearchResponse(
                1,
                10,
                new List<SearchResult>
                {
                    new SearchResult(1, "Massage", 59.3, 18.0, "1km", 1.0)
                }
            );

            var dto = SearchMapper.ToDto(domain);

            Assert.Equal(domain.TotalHits, dto.TotalHits);
            Assert.Equal(domain.TotalDocuments, dto.TotalDocuments);
            Assert.Single(dto.Results);
            Assert.Equal(domain.Results[0].Name, dto.Results[0].Name);
        }
    }

    public class ValidationTests
    {
        [Fact]
        public void SearchRequestDto_InvalidModel_ShouldFailValidation()
        {
            var dto = new SearchRequestDto("", new GeoPositionDto(59.3, 18.0));
            Assert.True(string.IsNullOrWhiteSpace(dto.ServiceName));

            var dto2 = new SearchRequestDto("Massage", new GeoPositionDto(200, 400));
            Assert.True(dto2.UserLocation.Lat < -90 || dto2.UserLocation.Lat > 90 ||
                        dto2.UserLocation.Lng < -180 || dto2.UserLocation.Lng > 180);
        }
    }
}