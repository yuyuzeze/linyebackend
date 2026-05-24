using Api.Extensions;
using Api.Logging;
using Api.Middleware;
using Api.DependencyInjection;
using Api.Interfaces;
using Api.Services;
using Api.Validators;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) =>
    {
        var logDirectory = context.Configuration["Serilog:LogDirectory"] ?? @"C:\Logs\app";
        Directory.CreateDirectory(logDirectory);

        var loggerConfig = configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.File(
                Path.Combine(logDirectory, "app-.log"),
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: SerilogDefaults.AppLogFileSizeBytes,
                outputTemplate: SerilogDefaults.OutputTemplate);

        if (context.HostingEnvironment.IsDevelopment())
        {
            loggerConfig
                .WriteTo.File(
                    Path.Combine(logDirectory, "debug_app-.log"),
                    restrictedToMinimumLevel: LogEventLevel.Debug,
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: SerilogDefaults.DebugLogFileSizeBytes,
                    outputTemplate: SerilogDefaults.OutputTemplate)
                .WriteTo.Console(outputTemplate: SerilogDefaults.OutputTemplate);
        }
    });

    builder.Services
        .AddControllers()
        .AddNewtonsoftJson();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddApplication();
    builder.Services.AddValidatorsFromAssemblyContaining<ClientLogEntryValidator>();
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
    builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
    builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("AuthDataSeeder");
        await AuthDataSeeder.SeedAsync(db, config, logger);
    }

    app.UseMiddleware<ExceptionHandlerMiddleware>();
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} が {StatusCode} で応答しました（{Elapsed:0.0000} ms）";
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("MessageId", AppMessageIds.HttpRequest);
            diagnosticContext.Set("RequestId", httpContext.TraceIdentifier);

            var upn = httpContext.RequestServices.GetService<ICurrentUserService>()?.Upn ?? string.Empty;
            diagnosticContext.Set("Upn", upn);
        };
    });
    app.UseSwagger();
    app.UseSwaggerUI();
    if (app.Environment.IsDevelopment())
        app.UseCors();
    app.UseRouting();
    app.UseAuthentication();
    app.UseMiddleware<RequestLogContextMiddleware>();
    app.UseMiddleware<RoleMiddleware>();
    app.UseAuthorization();
    app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
    app.MapControllers();

    Log.Information("API を起動しました。環境: {Environment}", app.Environment.EnvironmentName);
    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "API ホストが予期せず終了しました。");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
