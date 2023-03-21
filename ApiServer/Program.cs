using ApiServer;
using ApiServer.Hubs.SapceTelemetryHub;
// using Lightrun;

// LightrunAgent.Start(new AgentOptions {
//     Secret = "56e32e8e-f9d7-4c43-a250-dc426512aa75",
// });

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IConfiguration Configuration = builder.Configuration;
builder.Services.InitLogging(Configuration); //Инициализируем логер, без тэгов из настраиваемого файла, чтоб использовать при подключении файлоф инициализации
builder.Services.AddServices(Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapHub<TelemetryHub>("/TelemetryHub");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
