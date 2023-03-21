using Models.DTO;
using System.Threading.Tasks;

namespace ApiServer.Hubs.SapceTelemetryHub
{
    public interface ITelemetryHub
    {
        public Task GetGpsCoordinates(string json_gpsPhonePosition);
        public Task GetAccelerometer(string json_AccelerometerPhone);
        public Task GetGyroscopePhone(string json_GyroscopePhone);
        public Task GetMagnetometerPhone(string json_MagnetometerPhone);
    }
}
