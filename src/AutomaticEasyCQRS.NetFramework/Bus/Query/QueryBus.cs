using AutomaticEasyCQRS.NetFramework.Commands;
using AutomaticEasyCQRS.NetFramework.Queries;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace AutomaticEasyCQRS.NetFramework.Bus.Query
{
    public class QueryBus : IQueryBus
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public QueryBus(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<TResult> Query<TQuery, TResult>(TQuery query) where TQuery : IQuery where TResult : IQueryResult
        {
            using (var serviceScope = _serviceScopeFactory.CreateScope())
            {
                var handler = serviceScope.ServiceProvider.GetService<IQueryHandler<TQuery, TResult>>();

                if (handler == null)
                {
                    throw new InvalidOperationException($"No query handler found for {typeof(TQuery).Name}");
                }

                try
                {
                    return await handler.QueryHandle(query);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
