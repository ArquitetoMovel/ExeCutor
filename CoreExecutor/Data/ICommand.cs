using MediatR;

namespace ExeCutor
{
    public interface ICommand : IRequest<CommandResponse> { }

    public interface ICommand<TResponse> : IRequest<TResponse> where TResponse : ICommandResponse { }
}