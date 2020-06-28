using MediatR;
using System.Threading.Tasks;

namespace ExeCutor
{
    public class Executor 
    {
        private readonly IMediator _mediator;

        public Executor(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<CommandResponse> Send(ICommand command) => _mediator.Send<CommandResponse>(command);

        public Task<TResponse> Send<TResponse>(ICommand<TResponse> command) 
            where TResponse : ICommandResponse => _mediator.Send(command);

        public Task Publish(IEvent @event) => _mediator.Publish(@event);

    }
}
