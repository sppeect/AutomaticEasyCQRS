using AutomaticEasyCQRS.NetFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticEasyCQRS.NetFramework.Commands
{
    public interface ICommandHandler<TCommand> : IHandler where TCommand : ICommand
    {
        Task CommandHandle(TCommand command);
    }
}
