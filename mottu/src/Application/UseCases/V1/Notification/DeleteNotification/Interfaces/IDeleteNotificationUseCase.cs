using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Notification.DeleteNotification.Interfaces;

public interface IDeleteNotificationUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Notification> outputPort);
}