using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Order.DeleteOrder.Interfaces;

public interface IDeleteOrderUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Order> outputPort);
}