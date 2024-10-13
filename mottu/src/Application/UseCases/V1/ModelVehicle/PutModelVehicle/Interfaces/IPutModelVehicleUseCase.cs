using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.ModelVehicle.PutModelVehicle.Interfaces;

public interface IPutModelVehicleUseCase
{
    Task Execute(
        Domain.Entities.ModelVehicle modelVehicle);

    void SetOutputPort(
        IOutputPort<Domain.Entities.ModelVehicle> outputPort);
}