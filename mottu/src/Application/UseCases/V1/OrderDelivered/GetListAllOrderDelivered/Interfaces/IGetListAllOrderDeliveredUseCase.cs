using CrossCutting.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderDelivered.GetListAllOrderDelivered.Interfaces;

public interface IGetListAllOrderDeliveredUseCase
{
    Task Execute();

    void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.OrderDelivered>> outputPort);
}