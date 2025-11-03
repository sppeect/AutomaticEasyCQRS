using AutomaticEasyCQRS.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticEasyCQRS.Bus.Event
{
    public interface IEventBus
    {
        Task Publish<TEvent>(TEvent @event) where TEvent : IEvent;
        void Register<T>(IEventHandler<T> eventHandler, string topicOrQueue = null) where T : class, IEvent;
        void Unregister<T>(IEventHandler<T> eventHandler, string topicOrQueue = null) where T : class, IEvent;
        bool HasRegistered<T>() where T : class, IEvent;
    }
}
