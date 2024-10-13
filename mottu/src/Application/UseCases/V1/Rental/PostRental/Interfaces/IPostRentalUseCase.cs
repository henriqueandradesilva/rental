using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Rental.PostRental.Interfaces;

public interface IPostRentalUseCase
{
    Task Execute(
        Domain.Entities.Rental rental);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Rental> outputPort);
}