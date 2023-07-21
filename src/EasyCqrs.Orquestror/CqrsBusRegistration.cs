using EasyCqrs.Orquestror.Bus.Command;
using EasyCqrs.Orquestror.Bus.Event;
using EasyCqrs.Orquestror.Bus.Query;
using EasyCqrs.Orquestror.Commands;
using EasyCqrs.Orquestror.Events;
using EasyCqrs.Orquestror.Queries;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace EasyCqrs.Orquestror
{
    public static class CqrsBusRegistration
    {
        public static void RegisterBuses(this IServiceCollection services, Assembly assembly)
        {
            RegisterCommandBus(services, assembly);
            RegisterQueryBus(services, assembly);
            RegisterEventBus(services, assembly);
        }

        private static void RegisterCommandBus(IServiceCollection services, Assembly assembly)
        {
            // Finds all command handlers in the specified assembly and registers them
            var commandHandlerTypes = assembly.GetTypes().Where(t =>
                t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
            );

            foreach (var handlerType in commandHandlerTypes)
            {
                var commandInterface = handlerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
                services.AddTransient(commandInterface, handlerType);
            }

            // Register the Command Bus
            services.AddSingleton<ICommandBus, CommandBus>();
        }

        private static void RegisterQueryBus(IServiceCollection services, Assembly assembly)
        {
            // Finds all query handlers in the specified assembly and registers them
            var queryHandlerTypes = assembly.GetTypes().Where(t =>
                t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))
            );

            foreach (var handlerType in queryHandlerTypes)
            {
                var queryInterface = handlerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));
                services.AddTransient(queryInterface, handlerType);
            }

            // Register the Query Bus
            services.AddSingleton<IQueryBus, QueryBus>();
        }

        private static void RegisterEventBus(IServiceCollection services, Assembly assembly)
        {
            // Finds all event handlers in the specified assembly and registers them
            var eventHandlerTypes = assembly.GetTypes().Where(t =>
                t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>))
            );

            foreach (var handlerType in eventHandlerTypes)
            {
                var eventInterface = handlerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>));
                services.AddTransient(eventInterface, handlerType);
            }

            // Register the Event Bus
            services.AddSingleton<IEventBus, EventBus>();
        }
    }
}
