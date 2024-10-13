using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderDelivered.PutOrderDelivered.Interfaces;

public interface IPutOrderDeliveredUseCase
{
    Task Execute(
        Domain.Entities.OrderDelivered orderDelivered);

    void SetOutputPort(
        IOutputPort<Domain.Entities.OrderDelivered> outputPort);
}