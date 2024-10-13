using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderStatus.DeleteOrderStatus.Interfaces;

public interface IDeleteOrderStatusUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.OrderStatus> outputPort);
}