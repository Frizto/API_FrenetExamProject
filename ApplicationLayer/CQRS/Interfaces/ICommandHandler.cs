using ApplicationLayer.DTOs;

namespace ApplicationLayer.CQRS.Interfaces;
public interface ICommandHandler<TCommand, TResponse>
{
    Task<ServiceResponse> Handle(TCommand command, CancellationToken cancellationToken);
}