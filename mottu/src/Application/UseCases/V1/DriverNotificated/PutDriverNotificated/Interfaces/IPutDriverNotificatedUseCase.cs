using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.DriverNotificated.PutDriverNotificated.Interfaces;

public interface IPutDriverNotificatedUseCase
{
    Task Execute(
        Domain.Entities.DriverNotificated driverNotificated);

    void SetOutputPort(
        IOutputPort<Domain.Entities.DriverNotificated> outputPort);
}