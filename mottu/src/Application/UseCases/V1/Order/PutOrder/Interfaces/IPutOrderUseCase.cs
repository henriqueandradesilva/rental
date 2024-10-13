using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Order.PutOrder.Interfaces;

public interface IPutOrderUseCase
{
    Task Execute(
        Domain.Entities.Order order);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Order> outputPort);
}