# AutomaticEasyCQRS

O AutomaticEasyCQRS é uma biblioteca que visa facilitar a implementação do padrão CQRS (Command Query Responsibility Segregation) para desenvolvedores .NET. Com o AutomaticEasyCQRS, você pode criar e gerenciar facilmente os manipuladores de comandos, consultas e eventos, tornando a implementação do CQRS mais simples e organizada.

## O que é CQRS?

O CQRS é um padrão arquitetural que separa a lógica de leitura (queries) da lógica de gravação (commands) em uma aplicação. Ele promove a divisão de responsabilidades, permitindo um design mais limpo e uma melhor escalabilidade do sistema.

## Recursos

- Fácil mapeamento de comandos, consultas e eventos.
- Gerenciamento automático de manipuladores de CQRS.
- Suporte a injeção de dependência.
- Interface intuitiva para implementar manipuladores.

## Como usar o AutomaticEasyCQRS?

### Instalação

Para começar a usar o AutomaticEasyCQRS em seu projeto .NET, basta instalar o pacote NuGet:

```
dotnet add package AutomaticAutomaticEasyCQRS --version latest 
```
Ou ir diretamente no Nuget e pesquiar por AutomaticEasyCQRS.

### Exemplo de uso

Veja como é fácil implementar um manipulador de comando, consulta e evento.
Todos os Metodos podem ser Async ou não.

### Commands
```csharp
using System.Threading.Tasks;
using AutomaticEasyCQRS.Commands;

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
using AutomaticEasyCQRS.Queries;

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
using AutomaticEasyCQRS.Events;

public class Event : IEvent
{
    public Event(string userId)
    {
        UserId = userId;
    }
    public string UserId { get; }
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
### Controllers
``` csharp
using AutomaticEasyCQRS.Bus.Command;
using AutomaticEasyCQRS.Bus.Event;
using AutomaticEasyCQRS.Bus.Query;

 public class TesteController : ControllerBase
    {
        public readonly ICommandBus commandBus;
        public readonly IQueryBus queryBus;
        public readonly IEventBus eventBus;


        public TesteController(ICommandBus commandBus, IQueryBus queryBus, IEventBus eventBus)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;
            this.eventBus = eventBus;
        }

        [HttpGet]
        [Route("/teste/{q}")]
        public async Task<IActionResult> GetTest([FromRoute] string q)
        {

            // Command Syntax
            var handler = this.commandBus.Send(new TestCommand(q));
            // Query Syntax
            var queryHandler = this.queryBus.Query<TestQuery, TestResult>(new TestQuery(q));
            // Event Syntax
            var eventHandler = this.eventBus.Publish(new TestEvent(q));

            return Ok("Ok");
        }
    }
```


### Configuração

Para registrar automaticamente os manipuladores de CQRS, adicione o seguinte código na classe `Program.cs` do .NET 6:

Agora o usuario pode selecionar o tipo de instancia que ele quer criar (Scoped, Transient ou Singleton).
```csharp
public enum EHandlerInstanceType
    {
        Singleton,
        Scoped,
        Transient
    }
````

Por default é EHandlerInstanceType.Transient

```csharp
using AutomaticEasyCQRS;
using Microsoft.AspNetCore.Diagnostics;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
// Others Services
// Add services to the container.
...
CqrsBusRegistration.RegisterBuses(builder.Services, Assembly.GetExecutingAssembly(), EHandlerInstanceType.Scoped);
```

### Telemetria 

Para acessar a telemetria e salvar no banco de dados é necessario adicionar no seu mapeamento a classe

```csharp
AutomaticEasyCQRS.Telemetry.TelemetryStatistics;

// Exemplo Entity Framework Core
public DbSet<TelemetryStatistics> TelemetryStatistics { get; set; }
```

## Contribuindo

Contribuições são bem-vindas! Se você encontrar algum problema, tiver ideias para melhorias ou quiser contribuir com código, sinta-se à vontade para criar uma [issue](link-para-issues) ou enviar um [pull request](link-para-pull-requests).

## Licença

Este projeto está licenciado sob a [MIT License](link-da-licenca).

## Contato

Se você tiver alguma dúvida ou precisar de suporte, entre em contato conosco pelo email: [contato@jmdevelopment.dev](mailto:contato@jmdevelopment.dev).

Esperamos que o AutomaticEasyCQRS facilite a implementação do CQRS em seus projetos .NET. Aproveite e feliz codificação!


## Artigos que contribuiram para que eu criasse o AutomaticEasyCQRS
https://dejanstojanovic.net/aspnet/2019/may/using-dispatcher-class-to-resolve-commands-and-queries-in-aspnet-core/
https://dejanstojanovic.net/aspnet/2019/may/automatic-cqrs-handler-registration-in-aspnet-core-with-reflection/
https://event-driven.io/en/how_to_register_all_mediatr_handlers_by_convention/
