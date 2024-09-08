using MediatR;

namespace BuildingBlocks.Abstractions.CQRS
{
    public interface ICommand : ICommand<Unit>
    { }

    public interface ICommand<out TResponse> : IRequest<TResponse>
    { }
}