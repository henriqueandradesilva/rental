using Application.UseCases.V1.Motorcycle.DeleteMotorcycle.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Motorcycle.DeleteMotorcycle;

public class DeleteMotorcycleUseCase : IDeleteMotorcycleUseCase
{
    private IOutputPortWithNotFound<Domain.Entities.Motorcycle> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMotorcycleRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public DeleteMotorcycleUseCase(
        IUnitOfWork unitOfWork,
        IMotorcycleRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        long id)
    {
        var result =
            await _repository?.Where(c => c.Id == id)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.MotorcycleNotExist);

            _outputPort.NotFound();
        }
        else
        {
            _repository.Delete(result);

            var response =
                await _unitOfWork.Save()
                                 .ConfigureAwait(false);

            if (!string.IsNullOrEmpty(response))
            {
                _notificationHelper.Add(SystemConst.Error, response);

                _outputPort.Error();
            }
            else
                _outputPort.Ok(result);
        }
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Motorcycle> outputPort)
        => _outputPort = outputPort;
}