using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.DriverNotificated.PostDriverNotificated.Interfaces;

public interface IPostDriverNotificatedUseCase
{
    Task Execute(
        Domain.Entities.DriverNotificated driverNotificated);

    void SetOutputPort(
        IOutputPort<Domain.Entities.DriverNotificated> outputPort);
}