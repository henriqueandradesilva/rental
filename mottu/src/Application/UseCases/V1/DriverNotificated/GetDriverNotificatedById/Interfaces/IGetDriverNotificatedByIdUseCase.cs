using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.DriverNotificated.GetDriverNotificatedById.Interfaces;

public interface IGetDriverNotificatedByIdUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.DriverNotificated> outputPort);
}