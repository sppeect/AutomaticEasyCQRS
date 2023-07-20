using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCqrs.Contracts.Queries
{
    public interface IQueryHandler<TQuery, TResult> : IHandler where TQuery : IQuery where TResult : IQueryResult
    {
        // Esse codigo pode ser usado tanto para uso do ORM(Entity, NHibernate) quanto para o uso com Micro-ORM(Dapper)
        Task<TResult> QueryHandle(TQuery query);
    }
}
