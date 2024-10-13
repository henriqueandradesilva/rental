using CrossCutting.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UseCases.V1.DriverNotificated.GetListAllDriverNotificated.Interfaces;

public interface IGetListAllDriverNotificatedUseCase
{
    Task Execute();

    void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.DriverNotificated>> outputPort);
}