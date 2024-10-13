using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderAccepted.DeleteOrderAccepted.Interfaces;

public interface IDeleteOrderAcceptedUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.OrderAccepted> outputPort);
}