using WaruSmart.API.Crops.Domain.Model.Aggregates;
using WaruSmart.API.Crops.Domain.Model.Entities;
using WaruSmart.API.Shared.Domain.Repositories;

namespace WaruSmart.API.Crops.Domain.Repositories;

public interface ISowingRepository : IBaseRepository<Sowing>
{
    Task<IEnumerable<Sowing>> FindByStatusAsync(bool status);
    Task UpdateAsync(Sowing sowing);
    Task<IEnumerable<Product>> FindProductsBySowing(int sowingId);
    Task<IEnumerable<Sowing>> FindAllAsync();
    Task<IEnumerable<Sowing>> FindAllByUserIdAsync(int userId);
    
    Task<Sowing?> FindSowingByIdAsync(int id);
    
    Task<IEnumerable<Sowing>> FindAllWithDevicesAsync();
}