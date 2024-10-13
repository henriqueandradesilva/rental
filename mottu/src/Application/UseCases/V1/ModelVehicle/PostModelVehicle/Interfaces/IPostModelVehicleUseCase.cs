using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.ModelVehicle.PostModelVehicle.Interfaces;

public interface IPostModelVehicleUseCase
{
    Task Execute(
        Domain.Entities.ModelVehicle modelVehicle);

    void SetOutputPort(
        IOutputPort<Domain.Entities.ModelVehicle> outputPort);
}