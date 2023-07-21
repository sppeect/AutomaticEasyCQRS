using AutomaticEasyCQRS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticEasyCQRS.Bus.Command;

public interface ICommandBus
{
    Task Send<TCommand>(TCommand command) where TCommand : ICommand;
}

