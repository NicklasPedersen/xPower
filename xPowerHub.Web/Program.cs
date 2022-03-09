using xPowerHub;
using xPowerHub.DataStore;
using xPowerHub.Managers;
using xPowerHub.Managers.Interfaces;
using xPowerHub.Web.services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//IDataStore dds = new WizDS();
//IDataStore sds = new SmartThingsDS();
//IDataStorePower pds = new PowerDS();
//IDeviceManager dm = new DeviceManager(dds, sds)
//IPowerManager pm = new PowerManager(pds)
// Dependency injection
builder.Services.AddScoped<IDataStore<WizDevice>, WizDS>();
builder.Services.AddScoped<IDataStore<SmartThingsDevice>, SmartThingsDS>();
builder.Services.AddScoped<IDataStorePower, PowerDS>();

builder.Services.AddScoped<IDeviceManager, DeviceManager>();
builder.Services.AddScoped<IPowerManager, PowerManager>();

// Hosted services
builder.Services.AddHostedService<PowerUsageService>();

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
