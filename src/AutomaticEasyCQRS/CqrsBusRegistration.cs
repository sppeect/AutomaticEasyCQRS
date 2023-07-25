using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using AutomaticEasyCQRS.Bus.Command;
using AutomaticEasyCQRS.Bus.Event;
using AutomaticEasyCQRS.Bus.Query;
using AutomaticEasyCQRS.Commands;
using AutomaticEasyCQRS.Events;
using AutomaticEasyCQRS.Queries;
using AutomaticEasyCQRS.Telemetry;
using Microsoft.Extensions.DependencyInjection;

namespace AutomaticEasyCQRS
{

    public static class CqrsBusRegistration
    {
        // InstanceType will directly reflect how handlers and commands are executed in dependency injection, by default it is transient
        public static void RegisterBuses(this IServiceCollection services, Assembly assembly, EHandlerInstanceType instanceType = EHandlerInstanceType.Transient)
        {
            var assemblies = new List<Assembly> { assembly };
            assemblies.AddRange(assembly.GetReferencedAssemblies().Select(Assembly.Load));

            RegisterCommandBus(services, assemblies, instanceType);
            RegisterQueryBus(services, assemblies, instanceType);
            RegisterEventBus(services, assemblies, instanceType);

            services.AddScoped<TelemetryStatistics>();
        }
        private static void RegisterCommandBus(IServiceCollection services, IEnumerable<Assembly> assemblies, EHandlerInstanceType instanceType)
        {
            var commandHandlerTypes = assemblies.SelectMany(assembly => assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)))
            );

            var commandHandlerMap = new Dictionary<Type, List<Type>>();

            foreach (var handlerType in commandHandlerTypes)
            {
                var commandInterfaces = handlerType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
                foreach (var commandInterface in commandInterfaces)
                {
                    var commandType = commandInterface.GetGenericArguments()[0];

                    if (!commandHandlerMap.ContainsKey(commandType))
                    {
                        commandHandlerMap.Add(commandType, new List<Type>());
                    }
                    commandHandlerMap[commandType].Add(handlerType);
                }
            }
            foreach (var (commandType, handlerTypes) in commandHandlerMap)
            {
                foreach (var handlerType in handlerTypes)
                {
                    var addCommandHandlerMethod = typeof(CqrsOrquestror).GetMethod("AddCommandHandler", BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(commandType, handlerType);
                    addCommandHandlerMethod.Invoke(null, new object[] { services, instanceType });
                }
            }
            services.AddScoped<ICommandBus, CommandBus>();
        }


        private static void RegisterQueryBus(IServiceCollection services, IEnumerable<Assembly> assemblies, EHandlerInstanceType instanceType)
        {
            var queryHandlerTypes = assemblies.SelectMany(assembly => assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
            );

            var queryHandlerMap = new Dictionary<Type, Dictionary<Type, List<Type>>>();

            foreach (var handlerType in queryHandlerTypes)
            {
                var queryInterfaces = handlerType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));
                foreach (var queryInterface in queryInterfaces)
                {
                    var queryType = queryInterface.GetGenericArguments()[0];
                    var queryResultType = queryInterface.GetGenericArguments()[1];

                    if (!queryHandlerMap.ContainsKey(queryType))
                    {
                        queryHandlerMap.Add(queryType, new Dictionary<Type, List<Type>>());
                    }
                    var innerMap = queryHandlerMap[queryType];
                    if (!innerMap.ContainsKey(queryResultType))
                    {
                        innerMap.Add(queryResultType, new List<Type>());
                    }
                    innerMap[queryResultType].Add(handlerType);
                }
            }
            foreach (var (queryType, innerMap) in queryHandlerMap)
            {
                foreach (var (queryResultType, handlerTypes) in innerMap)
                {
                    foreach (var handlerType in handlerTypes)
                    {
                        var addQueryHandlerMethod = typeof(CqrsOrquestror).GetMethod("AddQueryHandler", BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(queryType, queryResultType, handlerType);

                        addQueryHandlerMethod.Invoke(null, new object[] { services, instanceType });
                    }
                }
            }
            services.AddScoped<IQueryBus, QueryBus>();
        }


        private static void RegisterEventBus(IServiceCollection services, IEnumerable<Assembly> assemblies, EHandlerInstanceType instanceType)
        {
            var eventHandlerTypes = assemblies.SelectMany(assembly => assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)))
            );
            var eventHandlerMap = new Dictionary<Type, List<Type>>();
            foreach (var handlerType in eventHandlerTypes)
            {
                var eventInterface = handlerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>));
                var eventType = eventInterface.GetGenericArguments()[0];

                if (!eventHandlerMap.ContainsKey(eventType))
                {
                    eventHandlerMap.Add(eventType, new List<Type>());
                }
                eventHandlerMap[eventType].Add(handlerType);
            }

            foreach (var (eventType, handlerTypes) in eventHandlerMap)
            {
                foreach (var handlerType in handlerTypes)
                {
                    var addEventHandlerMethod = typeof(CqrsOrquestror).GetMethod("AddEventHandler", BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(eventType, handlerType);
                    addEventHandlerMethod.Invoke(null, new object[] { services, instanceType });
                }
            }
            services.AddScoped<IEventBus, EventBus>();
        }

        
    }

}
