using AutomaticEasyCQRS.NetFramework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticEasyCQRS.NetFramework.Bus.Query
{
    public interface IQueryBus
    {
        Task<TResult> Query<TQuery, TResult>(TQuery query) where TQuery : IQuery where TResult : IQueryResult;
    }
}
