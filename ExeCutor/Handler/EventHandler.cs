using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ExeCutor
{
    public abstract class EventHandler<TEvent> : INotificationHandler<TEvent>
        where TEvent : IEvent
    {
        public Task Handle(TEvent @event, CancellationToken cancellationToken) =>
            HandleExecution(@event, cancellationToken);

        public abstract Task HandleExecution(TEvent @event, CancellationToken cancellationToken);
    }
}
