using AutomaticEasyCQRS.Commands;
using AutomaticEasyCQRS.Events;
using AutomaticEasyCQRS.Queries;
using AutomaticEasyCQRS;
using Microsoft.Extensions.DependencyInjection;
using AutomaticEasyCQRS.Telemetry;

public static class CqrsOrquestror
{
    private static readonly TelemetryStatistics _telemetryStatistics = new TelemetryStatistics();

    public static IServiceCollection AddCommandHandler<TCommand, TCommandHandler>(this IServiceCollection services, EHandlerInstanceType instanceType = EHandlerInstanceType.Transient)
        where TCommandHandler : class, ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        switch (instanceType)
        {
            case EHandlerInstanceType.Singleton:
                services.AddSingleton<TCommandHandler>();
                services.AddSingleton<ICommandHandler<TCommand>>(sp => sp.GetRequiredService<TCommandHandler>());
                break;
            case EHandlerInstanceType.Scoped:
                services.AddScoped<TCommandHandler>();
                services.AddScoped<ICommandHandler<TCommand>>(sp => sp.GetRequiredService<TCommandHandler>());
                break;
            case EHandlerInstanceType.Transient:
                services.AddTransient<TCommandHandler>();
                services.AddTransient<ICommandHandler<TCommand>>(sp => sp.GetRequiredService<TCommandHandler>());
                break;
            default:
                throw new ArgumentException("Invalid service lifetime selected.");
        }
        _telemetryStatistics.TotalCommandsRegistered++;
        return services;
    }

    public static IServiceCollection AddQueryHandler<TQuery, TQueryResult, TQueryHandler>(this IServiceCollection services, EHandlerInstanceType instanceType = EHandlerInstanceType.Transient)
        where TQueryHandler : class, IQueryHandler<TQuery, TQueryResult>
        where TQuery : IQuery
        where TQueryResult : IQueryResult
    {
        switch (instanceType)
        {
            case EHandlerInstanceType.Singleton:
                services.AddSingleton<TQueryHandler>();
                services.AddSingleton<IQueryHandler<TQuery, TQueryResult>>(sp => sp.GetRequiredService<TQueryHandler>());
                break;
            case EHandlerInstanceType.Scoped:
                services.AddScoped<TQueryHandler>();
                services.AddScoped<IQueryHandler<TQuery, TQueryResult>>(sp => sp.GetRequiredService<TQueryHandler>());
                break;
            case EHandlerInstanceType.Transient:
                services.AddTransient<TQueryHandler>();
                services.AddTransient<IQueryHandler<TQuery, TQueryResult>>(sp => sp.GetRequiredService<TQueryHandler>());
                break;
            default:
                throw new ArgumentException("Invalid service lifetime selected.");
        }
        _telemetryStatistics.TotalQueriesRegistered++;
        return services;
    }

    public static IServiceCollection AddEventHandler<TEvent, TEventHandler>(this IServiceCollection services, EHandlerInstanceType instanceType = EHandlerInstanceType.Transient)
        where TEventHandler : class, IEventHandler<TEvent>
        where TEvent : IEvent
    {
        switch (instanceType)
        {
            case EHandlerInstanceType.Singleton:
                services.AddSingleton<TEventHandler>();
                services.AddSingleton<IEventHandler<TEvent>>(sp => sp.GetRequiredService<TEventHandler>());
                break;
            case EHandlerInstanceType.Scoped:
                services.AddScoped<TEventHandler>();
                services.AddScoped<IEventHandler<TEvent>>(sp => sp.GetRequiredService<TEventHandler>());
                break;
            case EHandlerInstanceType.Transient:
                services.AddTransient<TEventHandler>();
                services.AddTransient<IEventHandler<TEvent>>(sp => sp.GetRequiredService<TEventHandler>());
                break;
            default:
                throw new ArgumentException("Invalid service lifetime selected.");
        }
        _telemetryStatistics.TotalEventsRegistered++;
        return services;
    }
}
