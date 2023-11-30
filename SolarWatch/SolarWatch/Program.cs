using SolarWatch.Client;
using SolarWatch.Processors;
using SolarWatch.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ISolarWatchClient, SolarWatchClient>();
builder.Services.AddScoped<ICityDataProvider, GeocodingApi>();
builder.Services.AddScoped<ISolarDataProvider, SunriseSunsetApi>();
builder.Services.AddScoped<ICityJsonProcessor, CityJsonProcessor>();
builder.Services.AddScoped<ISolarJsonProcessor, SolarJsonProcessor>();

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