using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderStatus.GetOrderStatusById.Interfaces;

public interface IGetOrderStatusByIdUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.OrderStatus> outputPort);
}