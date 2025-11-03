using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WaruSmart.API.Crops.Domain.Services;
using WaruSmart.API.Crops.Domain.Model.Queries;

namespace WaruSmart.API.Crops.Interfaces.REST
{
    [ApiController]
    [Route("api/v1/sowings")]
    [AllowAnonymous]
    public class SowingPhenologyController : ControllerBase
    {
        private readonly ISowingQueryService _sowingQueryService;

        public SowingPhenologyController(ISowingQueryService sowingQueryService)
        {
            _sowingQueryService = sowingQueryService;
        }

        [AllowAnonymous]
        [HttpGet("devices/phases")]
        public async Task<IActionResult> GetAllDevicesWithPhases()
        {
            try
            {
                var query = new GetAllDevicesWithPhasesQuery();
                var devices = await _sowingQueryService.Handle(query);

                var result = devices.Select(d => new
                {
                    device_id = d.DeviceId,
                    phenological_phase = d.Sowing?.PhenologicalPhase.ToString() ?? "Germination",
                    sowing_id = d.SowingId,
                    device_name = d.Name,
                    manually_active = d.ManuallyActive,
                    overwrite_automation = d.OverwriteAutomation,
                    last_sync = d.LastSyncDate
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("device/{deviceId}/phase")]
        public async Task<IActionResult> GetPhenologicalPhaseByDeviceId(string deviceId)
        {
            try
            {
                var query = new GetSowingByDeviceIdQuery(deviceId);
                var sowing = await _sowingQueryService.Handle(query);
                if (sowing == null)
                    return NotFound(new { error = "No sowing found for this device" });

                return Ok(new { phenological_phase = sowing.PhenologicalPhase });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}