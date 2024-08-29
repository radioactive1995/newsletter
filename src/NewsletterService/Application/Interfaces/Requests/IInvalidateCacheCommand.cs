using MediatR;

namespace Application.Interfaces.Requests;

public interface IInvalidateCacheCommand<out TResponse> : IRequest<TResponse>
{
    public string[] InvalidateKeys { get; }
}
