using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Motorcycle.PutMotorcycleSetRented.Interfaces;

public interface IPutMotorcycleSetRentedUseCase
{
    Task Execute(
        long id,
        bool rented);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Motorcycle> outputPort);
}