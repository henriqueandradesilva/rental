using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Driver.PutDriverSetDelivering.Interfaces;

public interface IPutDriverSetDeliveringUseCase
{
    Task Execute(
        long id,
        bool delivering);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Driver> outputPort);
}