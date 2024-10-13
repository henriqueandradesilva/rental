using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Notification.PostNotification.Interfaces;

public interface IPostNotificationUseCase
{
    Task Execute(
        Domain.Entities.Notification notification);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Notification> outputPort);
}