using Application.UseCases.V1.Driver.PutDriverSetDelivering.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Driver.PutDriverSetDelivering;

public class PutDriverSetDeliveringUseCase : IPutDriverSetDeliveringUseCase
{
    private IOutputPort<Domain.Entities.Driver> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDriverRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PutDriverSetDeliveringUseCase(
        IUnitOfWork unitOfWork,
        IDriverRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        long id,
        bool delivering)
    {
        var result =
            await _repository?.Where(c => c.Id == id)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.DriverNotExist);

            _outputPort.Error();
        }
        else
        {
            result.SetDelivering(delivering);

            _repository.Update(result);

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
        IOutputPort<Domain.Entities.Driver> outputPort)
        => _outputPort = outputPort;
}