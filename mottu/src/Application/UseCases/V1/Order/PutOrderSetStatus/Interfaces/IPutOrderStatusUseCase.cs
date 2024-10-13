using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Order.PutOrderSetStatus.Interfaces;

public interface IPutOrderSetStatusUseCase
{
    Task Execute(
        long id,
        long orderStatusId);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Order> outputPort);
}