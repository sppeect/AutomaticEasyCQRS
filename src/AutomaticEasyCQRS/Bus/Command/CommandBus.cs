using AutomaticEasyCQRS.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace AutomaticEasyCQRS.Bus.Command;
public class CommandBus : ICommandBus
{
    private readonly IServiceProvider _serviceProvider;

    public CommandBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Send<TCommand>(TCommand command) where TCommand : ICommand
    {
        var handler = _serviceProvider.GetService<ICommandHandler<TCommand>>();
        await handler.CommandHandle(command);
    }
}