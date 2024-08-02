namespace ApplicationLayer.CQRS.Interfaces;
public interface IQueryHandler<TQuery, TResultDTO>
{
    Task<TResultDTO> Handle(TQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<TResultDTO>> HandleListAsync(TQuery query, CancellationToken cancellationToken);
}
