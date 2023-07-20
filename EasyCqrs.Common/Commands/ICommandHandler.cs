using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCqrs.Contracts.Commands
{
    public interface ICommandHandler<TCommand> : IHandler where TCommand : ICommand
    {
        Task CommandHandle(TCommand command);
    }
}
