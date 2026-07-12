using System.Data;
using System.Diagnostics;
using Dapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DataAccess;

public sealed class DapperQueryGateway : IQueryGateway
{
    private readonly AppDbContext _db;
    private readonly ILogger<DapperQueryGateway> _logger;

    public DapperQueryGateway(AppDbContext db, ILogger<DapperQueryGateway> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IReadOnlyList<T>> QueryAsync<T>(
        string queryKey,
        string sql,
        object? param = null,
        CancellationToken cancellationToken = default)
    {
        var connection = await OpenConnectionAsync(cancellationToken);
        var sw = Stopwatch.StartNew();
        var rows = (await connection.QueryAsync<T>(
            new CommandDefinition(sql, param, cancellationToken: cancellationToken))).AsList();
        Log(queryKey, sw.ElapsedMilliseconds, rows.Count);
        return rows;
    }

    public async Task<T?> QuerySingleOrDefaultAsync<T>(
        string queryKey,
        string sql,
        object? param = null,
        CancellationToken cancellationToken = default)
    {
        var connection = await OpenConnectionAsync(cancellationToken);
        var sw = Stopwatch.StartNew();
        var row = await connection.QuerySingleOrDefaultAsync<T>(
            new CommandDefinition(sql, param, cancellationToken: cancellationToken));
        Log(queryKey, sw.ElapsedMilliseconds, row is null ? 0 : 1);
        return row;
    }

    public async Task<int> ExecuteAsync(
        string queryKey,
        string sql,
        object? param = null,
        CancellationToken cancellationToken = default)
    {
        var connection = await OpenConnectionAsync(cancellationToken);
        var sw = Stopwatch.StartNew();
        var affected = await connection.ExecuteAsync(
            new CommandDefinition(sql, param, cancellationToken: cancellationToken));
        Log(queryKey, sw.ElapsedMilliseconds, affected);
        return affected;
    }

    private async Task<IDbConnection> OpenConnectionAsync(CancellationToken cancellationToken)
    {
        var connection = _db.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
            await _db.Database.OpenConnectionAsync(cancellationToken);
        return connection;
    }

    private void Log(string queryKey, long elapsedMs, int rows)
    {
        _logger.LogInformation(
            "QueryKey={QueryKey} ElapsedMs={ElapsedMs} Rows={Rows}",
            queryKey,
            elapsedMs,
            rows);
    }
}
