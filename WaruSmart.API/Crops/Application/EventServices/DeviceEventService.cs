using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WaruSmart.API.Crops.Domain.Model.Entities;
using WaruSmart.API.Crops.Domain.Model.Events;
using WaruSmart.API.Crops.Domain.Services;

namespace WaruSmart.API.Crops.Application.EventServices
{
    public class DeviceEventService : IDeviceEventService
    {
        private readonly HttpClient _httpClient;
        private static readonly string EdgeAppUrl = Environment.GetEnvironmentVariable("EDGE_APP_URL") ?? "http://localhost:5000/api/events";            

        public DeviceEventService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> Handle(UpdateStatusDeviceEvent updateStatusEvent)
        {
            if (updateStatusEvent.DeviceId <= 0 || string.IsNullOrWhiteSpace(updateStatusEvent.Status))
            {
                Console.WriteLine($"[ERROR] The event has invalid data. DeviceId: {updateStatusEvent.DeviceId}, Status: '{updateStatusEvent.Status}'");
                return false;
            }
            try
            {
                var payload = JsonSerializer.Serialize(updateStatusEvent);
                Console.WriteLine($"[SIMULATION] Sending event to EDGE APP: {EdgeAppUrl}\nPayload: {payload}");
                // Successful simulation
                Console.WriteLine($"[SIMULATION] Event processed successfully for DeviceId: {updateStatusEvent.DeviceId}, Status: {updateStatusEvent.Status}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Exception while processing the event: {ex.Message}");
                return false;
            }
        }
    }
}
