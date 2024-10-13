using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Notification.GetNotificationById.Interfaces;

public interface IGetNotificationByIdUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Notification> outputPort);
}