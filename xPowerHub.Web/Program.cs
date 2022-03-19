using xPowerHub;
using xPowerHub.DataStore;
using xPowerHub.Managers;
using xPowerHub.Managers.Interfaces;
using xPowerHub.Managers.Test;
using xPowerHub.Repositories;
using xPowerHub.Repositories.Interfaces;
using xPowerHub.Web.services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency injection
#if TEST
builder.Services.AddScoped<IDeviceManager, DeviceManagerTest>();
builder.Services.AddScoped<IPowerManager, PowerManagerTest>();
#else
builder.Services.AddScoped<IDataStore<WizDevice>>(s => new WizDS(@".\xpower.db"));
builder.Services.AddScoped<IDataStore<SmartThingsDevice>>(s => new SmartThingsDS(@".\xpower.db"));
builder.Services.AddScoped<IDataStorePower>(s => new PowerDS(@".\xpower.db"));

builder.Services.AddScoped<IDeviceManager, DeviceManager>();
builder.Services.AddScoped<IPowerManager, PowerManager>();
builder.Services.AddScoped<IDeviceConnectionManager, DeviceConnectionManager>();

builder.Services.AddSingleton<IWizRepository, WizRepository>();

// Hosted services
builder.Services.AddHostedService<PowerUsageService>();
builder.Services.AddHostedService<DeviceSearchingService>();
#endif

// Move to config?
builder.WebHost.UseUrls("http://*:5000", "https://*:5001");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
