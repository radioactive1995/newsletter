using Domain.Subscribers;

namespace Application.Interfaces.Repositories;

public interface ISubscriberRepository
{
    Task<bool> DoesSubscriberExistWithEmail(string email);
    Task AddSubscriber(Subscriber subscriber);
    Task<int> GetSubscribersCount();
}
