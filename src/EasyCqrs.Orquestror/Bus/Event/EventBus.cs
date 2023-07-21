using EasyCqrs.Orquestror.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCqrs.Orquestror.Bus.Event
{
    public class EventBus : IEventBus
    {
        private readonly IServiceProvider _serviceProvider;

        public EventBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var handlers = _serviceProvider.GetServices<IEventHandler<TEvent>>();
            foreach (var handler in handlers)
            {
                await handler.EventHandle(@event);
            }
        }
    }
}
