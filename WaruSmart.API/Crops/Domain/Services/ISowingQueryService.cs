using WaruSmart.API.Crops.Domain.Model.Aggregates;
using WaruSmart.API.Crops.Domain.Model.Entities;
using WaruSmart.API.Crops.Domain.Model.Queries;

namespace WaruSmart.API.Crops.Domain.Services;

public interface ISowingQueryService
{
    Task<Sowing?> Handle(GetSowingByIdQuery query);
    Task<IEnumerable<Sowing>> Handle(GetSowingByStatusQuery query);
    
    Task<IEnumerable<Product>> Handle(GetProductsBySowingQuery query);
    Task<IEnumerable<Sowing>> Handle(GetAllSowingsQuery query);
    
    Task<IEnumerable<Sowing>> Handle(GetAllSowingsByUserIdQuery query);
    Task<Sowing?> Handle(GetSowingByDeviceIdQuery query);
    Task<IEnumerable<Device>> Handle(GetAllDevicesWithPhasesQuery query);
}