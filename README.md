# EasyCqrs

O EasyCqrs é uma biblioteca que visa facilitar a implementação do padrão CQRS (Command Query Responsibility Segregation) para desenvolvedores .NET. Com o EasyCqrs, você pode criar e gerenciar facilmente os manipuladores de comandos, consultas e eventos, tornando a implementação do CQRS mais simples e organizada.

## O que é CQRS?

O CQRS é um padrão arquitetural que separa a lógica de leitura (queries) da lógica de gravação (commands) em uma aplicação. Ele promove a divisão de responsabilidades, permitindo um design mais limpo e uma melhor escalabilidade do sistema.

## Recursos

- Fácil mapeamento de comandos, consultas e eventos.
- Gerenciamento automático de manipuladores de CQRS.
- Suporte a injeção de dependência.
- Interface intuitiva para implementar manipuladores.

## Como usar o EasyCqrs?

### Instalação

Para começar a usar o EasyCqrs em seu projeto .NET, basta instalar o pacote NuGet:

```
dotnet add package EasyCqrs
```

### Exemplo de uso

Veja como é fácil implementar um manipulador de comando usando o EasyCqrs:

### Commands
```csharp
using System.Threading.Tasks;
using EasyCqrs;

public class MyCommand : ICommand
{
    // Propriedades do comando
}

public class MyCommandHandler : ICommandHandler<MyCommand>
{
    public Task CommandHandle(MyCommand command)
    {
        // Lógica para manipular o comando
        return Task.CompletedTask;
    }
}
```
### Queries
```csharp
using System.Threading.Tasks;
using EasyCqrs;

public class MyQuery : IQuery
{
    // Propriedades da Query
}

public class MyQueryResult : IQueryResult
{
    // Propriedades do Result
}

public class MyQueryHandler : IQueryHandler<MyQuery, MyQueryResult>
{
    public async Task<MyQueryResult> QueryHandle(MyQuery query)
    {
        // Lógica para manipular o comando
        return new MyQueryResult();
    }
}
```
### Events
```csharp
using System.Threading.Tasks;
using EasyCqrs;

public class Event : IEvent
{
    public string UserId { get; }

    public Event(string userId)
    {
        UserId = userId;
    }
}

public class EventHandler : IEventHandler<Event>
{
    public Task EventHandle(Event @event)
    {
        // Lógica para manipular o evento Event
        Console.WriteLine($"Usuário registrado com ID: {@event.UserId}");
        return Task.CompletedTask;
    }
}
```

### Configuração

Para registrar automaticamente os manipuladores de CQRS, adicione o seguinte código na classe `Startup.cs` do ASP.NET Core:

```csharp
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Outras configurações...

        // Registre os manipuladores de comandos, consultas e eventos automaticamente.
        services.InjectHandlers(Assembly.GetExecutingAssembly());

        // Outras configurações...
    }
}
```


## Contribuindo

Contribuições são bem-vindas! Se você encontrar algum problema, tiver ideias para melhorias ou quiser contribuir com código, sinta-se à vontade para criar uma [issue](link-para-issues) ou enviar um [pull request](link-para-pull-requests).

## Licença

Este projeto está licenciado sob a [MIT License](link-da-licenca).

## Contato

Se você tiver alguma dúvida ou precisar de suporte, entre em contato conosco pelo email: [contact@sppeectDev.com](mailto:contact@sppeectDev.com).

Esperamos que o EasyCqrs facilite a implementação do CQRS em seus projetos .NET. Aproveite e feliz codificação!


## Artigos que contribuiram para que eu criasse o EasyCqrs
https://dejanstojanovic.net/aspnet/2019/may/using-dispatcher-class-to-resolve-commands-and-queries-in-aspnet-core/
https://dejanstojanovic.net/aspnet/2019/may/automatic-cqrs-handler-registration-in-aspnet-core-with-reflection/
https://event-driven.io/en/how_to_register_all_mediatr_handlers_by_convention/
