using CrossCutting.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderAccepted.GetListAllOrderAccepted.Interfaces;

public interface IGetListAllOrderAcceptedUseCase
{
    Task Execute();

    void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.OrderAccepted>> outputPort);
}