using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderDelivered.DeleteOrderDelivered.Interfaces;

public interface IDeleteOrderDeliveredUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.OrderDelivered> outputPort);
}