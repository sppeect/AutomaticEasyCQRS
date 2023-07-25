using System.Reflection;
using AutomaticEasyCQRS.Bus.Command;
using AutomaticEasyCQRS.Bus.Event;
using AutomaticEasyCQRS.Bus.Query;
using AutomaticEasyCQRS.Commands;
using AutomaticEasyCQRS.Events;
using AutomaticEasyCQRS.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace AutomaticEasyCQRS
{
    public static class CqrsBusRegistration
    {
        public static void RegisterBuses(this IServiceCollection services, Assembly assembly)
        {
            var assemblies = new List<Assembly> { assembly };
            assemblies.AddRange(assembly.GetReferencedAssemblies().Select(Assembly.Load));

            RegisterCommandBus(services, assemblies);
            RegisterQueryBus(services, assemblies);
            RegisterEventBus(services, assemblies);
        }

        private static void RegisterCommandBus(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            var commandHandlerTypes = assemblies.SelectMany(assembly => assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)))
            );

            foreach (var handlerType in commandHandlerTypes)
            {
                var commandInterface = handlerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
                services.AddTransient(commandInterface, handlerType);
            }

            services.AddSingleton<ICommandBus, CommandBus>();
        }

        private static void RegisterQueryBus(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            var queryHandlerTypes = assemblies.SelectMany(assembly => assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
            );

            foreach (var handlerType in queryHandlerTypes)
            {
                var queryInterface = handlerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));
                services.AddTransient(queryInterface, handlerType);
            }

            services.AddSingleton<IQueryBus, QueryBus>();
        }

        private static void RegisterEventBus(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            var eventHandlerTypes = assemblies.SelectMany(assembly => assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)))
            );

            foreach (var handlerType in eventHandlerTypes)
            {
                var eventInterface = handlerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>));
                services.AddTransient(eventInterface, handlerType);
            }

            services.AddSingleton<IEventBus, EventBus>();
        }
    }
}