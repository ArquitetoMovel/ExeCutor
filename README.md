# ExeCutor
---
A simple component that implements _CQRS pattern_ in .NET.

ExeCutor uses [mediatoR](https://github.com/jbogard/MediatR) component to create handlers for commands and
events.

The main goal is bringing an abstraction with Standardized Interfaces to
implements commands and integration events as well.

The package is built in **.NET STANDARD 2.0** to enable your utilization in **.NET
FRAMEWORK** and **.NET CORE**.

### Get Started


1.  Create a simple Command

    ```
    using ExeCutor;
    
    namespace CQRSImageDetails.Commands
    {
        public class RemoveImageCommand : ICommand
        {
            public int Id { get; set; }
        }
    }
    ``` 


2.  Create a Handler to you Command
    ```
    using ExeCutor;
    using CQRSImageDetails.Repository;
    using System.Threading;
    using System.Threading.Tasks;
    
    namespace CQRSImageDetails.Commands
    {
        public class RemoveImageCommandHandler : CommandHandler<RemoveImageCommand>
        {
            private readonly RepositoryPostgres _repository;
    
            public RemoveImageCommandHandler()
            {
                _repository = new RepositoryPostgres();
            }
    
            public override Task<CommandResponse> HandleExecution(RemoveImageCommand command, 
                                                                  CancellationToken cancellationToken) =>
            Task.Run<CommandResponse>(() =>
            {
                return new CommandResponse
                {
                    OK = _repository.DeleteImageDetails(command.Id)
                };
            });
        }
    }
    ```
### Using it with .NET FRAMEWORK

A pre requirement to create an instance of executor that will enable you
to send a Command or publish an Event is work with a container to
register all Handlers of your application.

The mediator also requires a container to register your interfaces.

If you already use a container IOC in your project, just use it, but if
not use I have a sample how to register with [AutoFac](https://github.com/autofac/Autofac) component.

#### Bellow we have a helper using AutoFac to register all Handler and your respective dependencies.
``` 
    private static void UseServices(Type[] commands,
                                    Type[] events,
                                    Action<Executor> configureExecutor)
    {
        var builder = new ContainerBuilder();
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();
        builder.RegisterType<Executor>();
        
        void registerHandler(Assembly assemblyHandler, Type typeHandler)
        {
            var registredHandler = builder
                                    .RegisterAssemblyTypes(assemblyHandler);
            registredHandler.AsClosedTypesOf(typeHandler);
            registredHandler.AsImplementedInterfaces();
        }
        
        foreach (var commandHandler in commands)
            registerHandler(commandHandler.GetTypeInfo().Assembly, typeof(IRequestHandler<,>));
        
        foreach (var eventHandler in events)
            registerHandler(eventHandler.GetTypeInfo().Assembly, typeof(INotificationHandler<>));
        
        builder.Register<ServiceFactory>(ctx =>
        {
            var c = ctx.Resolve<IComponentContext>();
            return t => c.Resolve(t);
        });
        
        
        var container = builder.Build();
        container.Resolve<IMediator>();
        
        if (configureExecutor != null)
            configureExecutor.Invoke(container.Resolve<Executor>());
        
    }
```

#### Now we can use the helper above
```
    Executor executorInstance = null;

    UseServices(
                new Type[]
                {
                    typeof(CreateNewImageCommand),
                    typeof(RemoveImageCommand)
                },
                new Type[]
                {
                    typeof(ImageCreatedEvent)
                },
                    (Executor instance) => { executorInstance = instance; }
                );

```
After registration steps, just create the commands and events and use
executor instance to send a command or publish an event.
```
    await executorInstance.Send(new RemoveImageCommand { Id = 1 });
```

> The complete sample solution is [here](https://github.com/ArquitetoMovel/CQRSImageDetails).
### 
