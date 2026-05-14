using CatalogService.Settings;
using CatalogService.Repositories;
using CatalogService.Repositories.Interfaces;
using CatalogService.Services;
using CatalogService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddScoped<IEventCatalogRepository,EventCatalogRepository>();
builder.Services.AddScoped<IEventCatalogService, EventCatalogManager>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

app.MapControllers();

app.UseHttpsRedirection();

app.Run();

