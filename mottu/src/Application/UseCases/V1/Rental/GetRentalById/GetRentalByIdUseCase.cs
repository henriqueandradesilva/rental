using Application.UseCases.V1.Rental.GetRentalById.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Rental.GetRentalById;

public class GetRentalByIdUseCase : IGetRentalByIdUseCase
{
    private IOutputPortWithForbid<Domain.Entities.Rental> _outputPort;
    private IRentalRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetRentalByIdUseCase(
        IRentalRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        long id,
        ClaimsPrincipal user)
    {
        var result =
            await _repository?.Where(c => c.Id == id)
                             ?.Include(c => c.Motorcycle).ThenInclude(c => c.ModelVehicle)
                             ?.Include(c => c.Driver).ThenInclude(c => c.User)
                             ?.Include(c => c.Plan).ThenInclude(c => c.PlanType)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.Forbid, MessageConst.RentalNotExist);

            _outputPort.Forbid();
        }
        else
        {
            if (!ValidateUserExtension.IsOwnerOrAdminByDriverId(result.DriverId, user))
                _outputPort.Forbid();
            else
                _outputPort.Ok(result);
        }
    }

    public void SetOutputPort(
        IOutputPortWithForbid<Domain.Entities.Rental> outputPort)
        => _outputPort = outputPort;

}