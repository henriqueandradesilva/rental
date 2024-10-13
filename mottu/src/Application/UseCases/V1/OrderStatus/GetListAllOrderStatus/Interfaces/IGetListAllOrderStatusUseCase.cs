using CrossCutting.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderStatus.GetListAllOrderStatus.Interfaces;

public interface IGetListAllOrderStatusUseCase
{
    Task Execute();

    void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.OrderStatus>> outputPort);
}