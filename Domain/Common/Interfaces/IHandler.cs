namespace Domain.Common.Interfaces;

public interface IHandler<TRequest, TResponse>
    where TRequest : IRequest
    where TResponse : IResponse
{
    Task<TResponse> Handle(TRequest request);
}