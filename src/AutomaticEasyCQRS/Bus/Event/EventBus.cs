using AutomaticEasyCQRS.Events;
using AutomaticEasyCQRS.Telemetry;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomaticEasyCQRS.Bus.Event
{
    public class EventBus : IEventBus
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly TelemetryStatistics _telemetryStatistics;

        public EventBus(IServiceScopeFactory serviceScopeFactory, TelemetryStatistics telemetryStatistics)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _telemetryStatistics = telemetryStatistics;
        }

        public async Task Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            using var scope = _serviceScopeFactory.CreateScope();

            try
            {
                var handlers = scope.ServiceProvider.GetServices<IEventHandler<TEvent>>();
                var handlerList = handlers?.ToList() ?? new List<IEventHandler<TEvent>>();

                if (!handlerList.Any())
                {
                    _telemetryStatistics.UpdateTelemetryStatistics(typeof(IEvent), false);
                    return;
                }

                var handlerTasks = handlerList.Select(handler => handler.EventHandle(@event));
                await Task.WhenAll(handlerTasks);

                _telemetryStatistics.UpdateTelemetryStatistics(typeof(IEvent), false);
            }
            catch (Exception ex)
            {
                _telemetryStatistics.UpdateTelemetryStatistics(typeof(IEvent), true, ex.Message);
                throw;
            }
        }
    }
}
