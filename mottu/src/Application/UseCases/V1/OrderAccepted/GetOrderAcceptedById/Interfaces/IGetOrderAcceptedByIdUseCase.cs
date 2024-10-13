using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderAccepted.GetOrderAcceptedById.Interfaces;

public interface IGetOrderAcceptedByIdUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.OrderAccepted> outputPort);
}