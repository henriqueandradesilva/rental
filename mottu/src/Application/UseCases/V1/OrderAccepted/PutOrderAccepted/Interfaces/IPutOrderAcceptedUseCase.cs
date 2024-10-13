using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderAccepted.PutOrderAccepted.Interfaces;

public interface IPutOrderAcceptedUseCase
{
    Task Execute(
        Domain.Entities.OrderAccepted orderAccepted);

    void SetOutputPort(
        IOutputPort<Domain.Entities.OrderAccepted> outputPort);
}