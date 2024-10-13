using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.DriverNotificated.DeleteDriverNotificated.Interfaces;

public interface IDeleteDriverNotificatedUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.DriverNotificated> outputPort);
}