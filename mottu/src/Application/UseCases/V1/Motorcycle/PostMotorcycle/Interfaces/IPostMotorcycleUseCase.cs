using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Motorcycle.PostMotorcycle.Interfaces;

public interface IPostMotorcycleUseCase
{
    Task Execute(
        Domain.Entities.Motorcycle motorcycle,
        string modelVehicleDescription);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Motorcycle> outputPort);
}