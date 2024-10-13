using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderDelivered.PostOrderDelivered.Interfaces;

public interface IPostOrderDeliveredUseCase
{
    Task Execute(
        Domain.Entities.OrderDelivered orderDelivered);

    void SetOutputPort(
        IOutputPort<Domain.Entities.OrderDelivered> outputPort);
}