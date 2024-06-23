using AutomaticEasyCQRS.NetFramework.Commands;
using AutomaticEasyCQRS.NetFramework.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticEasyCQRS.NetFramework.Bus.Event
{
    public class EventBus : IEventBus
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EventBus(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var handlers = scope.ServiceProvider.GetServices<IEventHandler<TEvent>>();
                    foreach (var handler in handlers)
                    {
                        await handler.EventHandle(@event);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
