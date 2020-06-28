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
}
