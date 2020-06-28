using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ExeCutor
{
    public abstract class CommandHandler<TCommand> : IRequestHandler<TCommand, CommandResponse>
        where TCommand : ICommand
    {
        public Task<CommandResponse> Handle(TCommand request, CancellationToken cancellationToken) =>
        HandleExecution(request, cancellationToken);

        public abstract Task<CommandResponse> HandleExecution(TCommand command, CancellationToken cancellationToken);
    }

    public abstract class CommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
      where TCommand : ICommand<TResponse>
      where TResponse : ICommandResponse
    {
        public abstract Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
    }
}
