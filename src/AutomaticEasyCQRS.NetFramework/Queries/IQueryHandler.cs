using AutomaticEasyCQRS.NetFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticEasyCQRS.NetFramework.Queries
{
    public interface IQueryHandler<TQuery, TResult> : IHandler where TQuery : IQuery where TResult : IQueryResult
    {
        Task<TResult> QueryHandle(TQuery query);
    }
}
