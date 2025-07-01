using SearchService.Infra.Entities;

namespace SearchService.Infra.Interfaces
{
    public interface IServiceRepository
    {
        Task<IEnumerable<ServiceEntity>> GetAllAsync();
    }
}