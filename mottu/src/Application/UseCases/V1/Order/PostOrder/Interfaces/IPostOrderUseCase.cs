using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Order.PostOrder.Interfaces;

public interface IPostOrderUseCase
{
    Task Execute(
        Domain.Entities.Order order);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Order> outputPort);
}