using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Order.GetOrderById.Interfaces;

public interface IGetOrderByIdUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Order> outputPort);
}