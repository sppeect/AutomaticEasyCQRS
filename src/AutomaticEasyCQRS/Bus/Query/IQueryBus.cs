﻿using AutomaticEasyCQRS.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticEasyCQRS.Bus.Query
{
    public interface IQueryBus
    {
        Task<TResult> Query<TQuery, TResult>(TQuery query) where TQuery : IQuery where TResult : IQueryResult;
    }
}
