using Api.Extensions;
using Api.Services;
using Application.DependencyInjection;
using Application.Interfaces;
using Application.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    builder.Services
        .AddControllers()
        .AddNewtonsoftJson();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddApplication();
    builder.AddApiInfrastructure();
    builder.AddApiAuthentication();

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

    app.UseSerilogRequestLogging();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
    app.MapControllers();

    Log.Information("API started. Environment: {Environment}", app.Environment.EnvironmentName);
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "API host terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
