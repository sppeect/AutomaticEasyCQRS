using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EasyCqrs.Orquestror
{
    public static class CqrsAutomaticOrquestror
    {
        public static void InjectHandlers(this IServiceCollection services, Assembly assembly)
        {
            //Finds all classes that implement the IHandler interfaces in the specified assembly
            var handlerTypes = assembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandler)));

            // Register each handler found in the dependency injection container
            foreach (var handlerType in handlerTypes)
            {
                var handlerInterfaces = handlerType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandler));
                foreach (var handlerInterface in handlerInterfaces)
                {
                    services.AddTransient(handlerInterface, handlerType);
                }
            }
        }
    }
}
