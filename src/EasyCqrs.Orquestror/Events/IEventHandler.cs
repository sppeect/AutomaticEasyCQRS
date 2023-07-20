using EasyCqrs.Orquestror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCqrs.Orquestror.Events
{
    public interface IEventHandler<TEvent> : IHandler where TEvent : IEvent
    {
        Task EventHandle(TEvent command);

    }
}
