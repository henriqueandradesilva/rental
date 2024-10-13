using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Motorcycle.PutMotorcycleSetPlate.Interfaces;

public interface IPutMotorcycleSetPlateUseCase
{
    Task Execute(
        long id,
        string plate);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Motorcycle> outputPort);
}