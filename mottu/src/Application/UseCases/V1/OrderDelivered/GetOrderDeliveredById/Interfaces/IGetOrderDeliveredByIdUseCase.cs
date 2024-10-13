using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderDelivered.GetOrderDeliveredById.Interfaces;

public interface IGetOrderDeliveredByIdUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.OrderDelivered> outputPort);
}