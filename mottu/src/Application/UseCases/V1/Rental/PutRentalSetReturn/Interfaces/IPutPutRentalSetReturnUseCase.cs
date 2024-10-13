using CrossCutting.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Rental.PutRentalSetReturn.Interfaces;

public interface IPutRentalSetReturnUseCase
{
    Task Execute(
        long id,
        DateTime endDate,
        ClaimsPrincipal user);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Rental> outputPort);
}