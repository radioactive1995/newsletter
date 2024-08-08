using MediatR;

namespace Application.Interfaces.Requests;

public interface IInvalidateCacheCommand<TResponse> : IRequest<TResponse>
{
    public string[] InvalidateKeys { get; }
}
