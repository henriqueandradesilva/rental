using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Rental.PutRental.Interfaces;

public interface IPutRentalUseCase
{
    Task Execute(
        Domain.Entities.Rental rental);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Rental> outputPort);
}