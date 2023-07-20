using EasyCqrs.Orquestror.Commands;
using EasyCqrs.Orquestror.Events;
using EasyCqrs.Orquestror.Queries;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCqrs.Orquestror
{
    public static class CqrsManualOrquestror
    {
        // This code will register your handlers manually, one by one, inside your startup or container.
        public static IServiceCollection AddCommandHandler<TCommand, TCommandHandler>(
            this IServiceCollection services
        )
            where TCommandHandler : class, ICommandHandler<TCommand>
            where TCommand : ICommand
        {
            return services.AddTransient<ICommandHandler<TCommand>, TCommandHandler>();
        }

        public static IServiceCollection AddQueryHandler<TQuery, TQueryResult, TQueryHandler>(
            this IServiceCollection services
        )
            where TQueryHandler : class, IQueryHandler<TQuery, TQueryResult>
            where TQuery : IQuery
            where TQueryResult : IQueryResult
        {
            return services.AddTransient<IQueryHandler<TQuery, TQueryResult>, TQueryHandler>();
        }

        public static IServiceCollection AddEventHandler<TEvent, TEventHandler>(
            this IServiceCollection services
        )
            where TEventHandler : class, IEventHandler<TEvent>
            where TEvent : IEvent
        {
            return services.AddTransient<IEventHandler<TEvent>, TEventHandler>();
        }
    }

}
