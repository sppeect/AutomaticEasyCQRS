using AutomaticEasyCQRS.NetFramework.Bus.Command;
using AutomaticEasyCQRS.NetFramework.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

public class CommandBus : ICommandBus
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CommandBus(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Send<TCommand>(TCommand command) where TCommand : ICommand
    {
        try
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var handler = scope.ServiceProvider.GetService<ICommandHandler<TCommand>>();
                await handler.CommandHandle(command);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}