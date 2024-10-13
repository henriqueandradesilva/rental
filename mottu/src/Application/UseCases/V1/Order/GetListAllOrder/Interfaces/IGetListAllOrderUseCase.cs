using CrossCutting.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Order.GetListAllOrder.Interfaces;

public interface IGetListAllOrderUseCase
{
    Task Execute();

    void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.Order>> outputPort);
}