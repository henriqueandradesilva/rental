using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Rental.DeleteRental.Interfaces;

public interface IDeleteRentalUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Rental> outputPort);
}