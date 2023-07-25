using AutomaticEasyCQRS.Commands;
using AutomaticEasyCQRS.Queries;
using AutomaticEasyCQRS.Telemetry;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AutomaticEasyCQRS.Bus.Query;

public class QueryBus : IQueryBus
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly TelemetryStatistics _telemetryStatistics;

    public QueryBus(IServiceScopeFactory serviceScopeFactory, TelemetryStatistics telemetryStatistics)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _telemetryStatistics = telemetryStatistics;
    }

    public async Task<TResult> Query<TQuery, TResult>(TQuery query) where TQuery : IQuery where TResult : IQueryResult
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var handler = scope.ServiceProvider.GetService<IQueryHandler<TQuery, TResult>>();

        if (handler == null)
        {
            throw new InvalidOperationException($"No query handler found for {typeof(TQuery).Name}");
        }
        try
        {
            _telemetryStatistics.UpdateTelemetryStatistics(typeof(IQuery), false);
            return await handler.QueryHandle(query);
        }
        catch (Exception ex)
        {
            _telemetryStatistics.UpdateTelemetryStatistics(typeof(IQuery), true, ex.Message);
            return default;
        }
    }
}
