using System;
using System.Collections.Generic;
using System.Linq;
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
        public static void RegisterBuses(this IServiceCollection services, Assembly assembly, EHandlerInstanceType instanceType)
        {
            var assemblies = new List<Assembly> { assembly };
            assemblies.AddRange(assembly.GetReferencedAssemblies().Select(Assembly.Load));

            RegisterCommandBus(services, assemblies, instanceType);
            RegisterQueryBus(services, assemblies, instanceType);
            RegisterEventBus(services, assemblies, instanceType);
        }

        private static void RegisterCommandBus(IServiceCollection services, IEnumerable<Assembly> assemblies, EHandlerInstanceType instanceType)
        {
            var commandHandlerTypes = assemblies.SelectMany(assembly => assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)))
            );

            foreach (var handlerType in commandHandlerTypes)
            {
                var commandInterface = handlerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>));

                switch (instanceType)
                {
                    case EHandlerInstanceType.Singleton:
                        services.AddSingleton(commandInterface, handlerType);
                        break;
                    case EHandlerInstanceType.Scoped:
                        services.AddScoped(commandInterface, handlerType);
                        break;
                    case EHandlerInstanceType.Transient:
                        services.AddTransient(commandInterface, handlerType);
                        break;
                    default:
                        throw new ArgumentException("Invalid instance type selected.");
                }
            }

            services.AddSingleton<ICommandBus, CommandBus>();
        }

        private static void RegisterQueryBus(IServiceCollection services, IEnumerable<Assembly> assemblies, EHandlerInstanceType instanceType)
        {
            var queryHandlerTypes = assemblies.SelectMany(assembly => assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
            );

            foreach (var handlerType in queryHandlerTypes)
            {
                var queryInterface = handlerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));

                switch (instanceType)
                {
                    case EHandlerInstanceType.Singleton:
                        services.AddSingleton(queryInterface, handlerType);
                        break;
                    case EHandlerInstanceType.Scoped:
                        services.AddScoped(queryInterface, handlerType);
                        break;
                    case EHandlerInstanceType.Transient:
                        services.AddTransient(queryInterface, handlerType);
                        break;
                    default:
                        throw new ArgumentException("Invalid instance type selected.");
                }
            }

            services.AddSingleton<IQueryBus, QueryBus>();
        }

        private static void RegisterEventBus(IServiceCollection services, IEnumerable<Assembly> assemblies, EHandlerInstanceType instanceType)
        {
            var eventHandlerTypes = assemblies.SelectMany(assembly => assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)))
            );

            foreach (var handlerType in eventHandlerTypes)
            {
                var eventInterface = handlerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>));

                switch (instanceType)
                {
                    case EHandlerInstanceType.Singleton:
                        services.AddSingleton(eventInterface, handlerType);
                        break;
                    case EHandlerInstanceType.Scoped:
                        services.AddScoped(eventInterface, handlerType);
                        break;
                    case EHandlerInstanceType.Transient:
                        services.AddTransient(eventInterface, handlerType);
                        break;
                    default:
                        throw new ArgumentException("Invalid instance type selected.");
                }
            }

            services.AddSingleton<IEventBus, EventBus>();
        }
    }
}
