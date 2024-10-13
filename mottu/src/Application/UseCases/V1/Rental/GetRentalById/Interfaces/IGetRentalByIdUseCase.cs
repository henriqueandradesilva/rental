using CrossCutting.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Rental.GetRentalById.Interfaces;

public interface IGetRentalByIdUseCase
{
    Task Execute(
        long id,
        ClaimsPrincipal user);

    void SetOutputPort(
        IOutputPortWithForbid<Domain.Entities.Rental> outputPort);
}