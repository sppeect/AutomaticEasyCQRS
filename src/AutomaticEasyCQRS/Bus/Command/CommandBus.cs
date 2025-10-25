using AutomaticEasyCQRS.Bus.Command;
using AutomaticEasyCQRS.Commands;
using AutomaticEasyCQRS.Telemetry;
using Microsoft.Extensions.DependencyInjection;
using System;

public class CommandBus : ICommandBus
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly TelemetryStatistics _telemetryStatistics;

    public CommandBus(IServiceScopeFactory serviceScopeFactory, TelemetryStatistics telemetryStatistics)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _telemetryStatistics = telemetryStatistics;
    }

    public async Task Send<TCommand>(TCommand command) where TCommand : ICommand
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var handler = scope.ServiceProvider.GetService<ICommandHandler<TCommand>>();

            if (handler == null)
            {
                throw new InvalidOperationException($"No command handler found for {typeof(TCommand).Name}");
            }

            await handler.CommandHandle(command);

            _telemetryStatistics.UpdateTelemetryStatistics(typeof(ICommand), false);
        }
        catch (Exception ex)
        {
            _telemetryStatistics.UpdateTelemetryStatistics(typeof(ICommand), true, ex.Message);
            throw;
        }
    }
}
