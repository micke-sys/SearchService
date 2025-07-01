using SearchService.BL.Interfaces;
using SearchService.Infra.Interfaces;
using SearchService.Infra.Repositories;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build())
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

// Replace default logging with Serilog
builder.Host.UseSerilog((context, services, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ISearchService, SearchService.BL.SearchApplicationService>();

var dataFilePath = builder.Configuration.GetValue<string>("DataFile");

builder.Services.AddSingleton<IServiceRepository>(sp =>
    new JsonServiceRepository(
        Path.Combine(AppContext.BaseDirectory, dataFilePath),
        sp.GetRequiredService<ILogger<JsonServiceRepository>>()
    ));

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