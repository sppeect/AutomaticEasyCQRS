using EasyCqrs.Orquestror.Bus.Query;
using EasyCqrs.Orquestror.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace EasyCqrs.Orquestror.Bus.Query;

public class QueryBus : IQueryBus
{
    private readonly IServiceProvider _serviceProvider;

    public QueryBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResult> Query<TQuery, TResult>(TQuery query) where TQuery : IQuery where TResult : IQueryResult
    {
        // Verifica se existe um manipulador registrado para a consulta TQuery

        if (_serviceProvider.GetService(typeof(IQueryHandler<TQuery, TResult>)) is not IQueryHandler<TQuery, TResult> handler)
        {
            throw new InvalidOperationException($"No query handler found for {typeof(TQuery).Name}");
        }

        return await handler.QueryHandle(query);
    }
}
