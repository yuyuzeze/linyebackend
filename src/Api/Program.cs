using Application.Interfaces;
using Application.Services;
using Api.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=(localdb)\\mssqllocaldb;Database=linye;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IDemoItemRepository, DemoItemRepository>();
builder.Services.AddScoped<IDemoItemService, DemoItemService>();
builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
builder.Services.AddScoped<IProcessedBlobRecordRepository, ProcessedBlobRecordRepository>();
builder.Services.AddScoped<IVoucherImportService, VoucherImportService>();
builder.Services.AddSingleton<IBlobStorageService, BlobStorageService>();
builder.Services.AddScoped<IApplicationTypeRepository, ApplicationTypeRepository>();
builder.Services.AddScoped<IApplicationTypeFieldRepository, ApplicationTypeFieldRepository>();
builder.Services.AddScoped<ICsvColumnMappingRepository, CsvColumnMappingRepository>();
builder.Services.AddScoped<IApplicationTypeService, ApplicationTypeService>();
builder.Services.AddScoped<ICsvMappingService, CsvMappingService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "https://blue-cliff-0e4f85d00.2.azurestaticapps.net"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var startupLogger = scope.ServiceProvider
        .GetRequiredService<ILoggerFactory>()
        .CreateLogger("Startup");
    try
    {
        var sqlConnectionBuilder = new SqlConnectionStringBuilder(connectionString);
        startupLogger.LogInformation(
            "Initializing database. Server: {Server}; Database: {Database}",
            sqlConnectionBuilder.DataSource,
            sqlConnectionBuilder.InitialCatalog);

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();
        startupLogger.LogInformation("Database migration completed.");
    }
    catch (Exception ex)
    {
        startupLogger.LogCritical(ex, "Database initialization failed.");
        throw;
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
