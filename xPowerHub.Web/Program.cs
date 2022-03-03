using xPowerHub.DataStore;
using xPowerHub.Managers;
using xPowerHub.Managers.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency injection
builder.Services.AddSingleton<IDataStore>(new DAL("./xPower.db"));
builder.Services.AddSingleton<IDeviceManager>(x => new DeviceManager(x.GetRequiredService<IDataStore>()));

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

app.UseAuthorization();

app.MapControllers();

app.Run();
