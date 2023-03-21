using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Logging;
using Mars;
using Models.DTO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Features;
using Database.Contexts.Services;


namespace ApiServer.Hubs.SapceTelemetryHub
{
    public class TelemetryHub : Hub<ITelemetryHub>
    {
        private readonly DbContextService _dbContextService;
        public TelemetryHub(
            DbContextService dbContextService
        )
        {
            _dbContextService = dbContextService;
        }
        
        public override async Task OnConnectedAsync()
        {
            var remouteAdress = GetRemouteIp();

             // get relative client info from headers
            var host = Context.GetHttpContext()!.Request.Headers["Host"];
            var userAgent = Context.GetHttpContext()!.Request.Headers["User-Agent"];
            var realIP = Context.GetHttpContext()!.Request.Headers["X-Real-IP"];
            var forwarded = Context.GetHttpContext()!.Request.Headers["X-Forwarded-For"]; 
            var connectedInfo = new Dictionary<string, string> () { 
                { "Host", host }, 
                { "UserAgent", userAgent }, 
                { "Real-IP", realIP }, 
                { "Forward-For", forwarded },
            };

            await base.OnConnectedAsync();
            Log.InformationAsync($"App hub connect: {remouteAdress}").DisableAsyncWarning();
            Log.InformationAsync($"Info: {JsonConvert.SerializeObject(connectedInfo)}").DisableAsyncWarning();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var remouteAdress = GetRemouteIp();

            await base.OnDisconnectedAsync(exception);
            Log.InformationAsync($"App hub disconnect: {remouteAdress}").DisableAsyncWarning();
        }

        public async Task GetGpsCoordinates(string json_gpsPhonePosition)
        {
            GPSPhonePosition? gpsPhonePosition = JsonConvert.DeserializeObject<GPSPhonePosition>(json_gpsPhonePosition);
            Log.Information($"GetGpsCoordinates : {gpsPhonePosition!.toJSON()}");

            var user = await _dbContextService.setUserAsync(GetRemouteIp());
            await _dbContextService.saveGPSAsync(gpsPhonePosition,user);
        }

        public async Task GetAccelerometer(string json_AccelerometerPhone)
        {
            AccelerometerPhone? accelerometer = JsonConvert.DeserializeObject<AccelerometerPhone>(json_AccelerometerPhone);
            //Log.Information($"GetAccelerometer : {accelerometer!.toJSON()}");
            await Task.CompletedTask;
        }

        public async Task GetGyroscopePhone(string json_GyroscopePhone)
        {
            GyroscopePhone? gyroscope = JsonConvert.DeserializeObject<GyroscopePhone>(json_GyroscopePhone);
            //Log.Information($"GetGyroscopePhone : {gyroscope!.toJSON()}");
            await Task.CompletedTask;
        }

        public async Task GetMagnetometerPhone(string json_MagnetometerPhone)
        {
            MagnetometerPhone? gyroscope = JsonConvert.DeserializeObject<MagnetometerPhone>(json_MagnetometerPhone);
            //Log.Information($"GetMagnetometerPhone : {gyroscope!.toJSON()}");
            await Task.CompletedTask;
        }

        private string GetRemouteIp()
        {
            var feature = Context.Features.Get<IHttpConnectionFeature>();
            var remouteAdress = feature is not null ? feature.RemoteIpAddress!.MapToIPv4().ToString() : "";   
            return remouteAdress;
        }
    }
}
