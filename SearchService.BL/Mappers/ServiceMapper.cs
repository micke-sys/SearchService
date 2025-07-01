using SearchService.BL.DomainModels;
using SearchService.Infra.Entities;

namespace SearchService.BL.Mappers;

public static class ServiceMapper
{
    public static Service ToDomain(ServiceEntity entity)
        => new Service(
            entity.Id,
            entity.Name ?? string.Empty,
            new Position
            {
                Lat = entity.Position.Lat,
                Lng = entity.Position.Lng
            }
        );
}