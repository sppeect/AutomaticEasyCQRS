using AutomaticEasyCQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticEasyCQRS.Events
{
    public interface IEventHandler<TEvent> : IHandler where TEvent : IEvent
    {
        Task EventHandle(TEvent command);

    }
}
