using MediatR;

namespace Application.Interfaces.Requests;

public interface ICachedQuery<TResponse> : IRequest<TResponse>
{
    public string Key { get; }
}
