using WaruSmart.API.Crops.Domain.Repositories;
using WaruSmart.API.Resources.Domain.Repositories;
using WaruSmart.API.Resources.Domain.Services;
using WaruSmart.API.Shared.Domain.Repositories;

namespace WaruSmart.API.Resources.Application.InboundServices;

public class FogSyncService:IFogSyncService
{
    private readonly IIoTDataRepository _repository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly ISowingRepository _sowingRepository;

    public FogSyncService(IIoTDataRepository repository,
        IDeviceRepository deviceRepository,
        ISowingRepository sowingRepository)
    {
        _repository = repository;
        _deviceRepository = deviceRepository;
        _sowingRepository = sowingRepository;
    }
    
    private async Task UpdateData()
    {
        await _repository.FogAsync();
    }

    public async Task SyncFogDataAsync()
    {
        await _repository.FogAsync();
    }
}