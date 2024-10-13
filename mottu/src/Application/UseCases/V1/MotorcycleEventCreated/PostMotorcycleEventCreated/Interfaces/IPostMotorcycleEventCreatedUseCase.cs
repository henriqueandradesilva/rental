using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.MotorcycleEventCreated.PostMotorcycleEventCreated.Interfaces;

public interface IPostMotorcycleEventCreatedUseCase
{
    Task Execute(
        Domain.Entities.MotorcycleEventCreated motorcycleEventCreated);

    void SetOutputPort(
        IOutputPort<Domain.Entities.MotorcycleEventCreated> outputPort);
}