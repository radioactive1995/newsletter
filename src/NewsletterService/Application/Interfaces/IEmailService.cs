namespace Application.Interfaces;

public interface IEmailService
{
    Task SendSubscriberConfirmationMail();
}
