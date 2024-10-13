using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderStatus.PostOrderStatus.Interfaces;

public interface IPostOrderStatusUseCase
{
    Task Execute(
        Domain.Entities.OrderStatus orderStatus);

    void SetOutputPort(
        IOutputPort<Domain.Entities.OrderStatus> outputPort);
}