using Application.UseCases.V1.OrderStatus.DeleteOrderStatus.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderStatus.DeleteOrderStatus;

public class DeleteOrderStatusUseCase : IDeleteOrderStatusUseCase
{
    private IOutputPortWithNotFound<Domain.Entities.OrderStatus> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderStatusRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public DeleteOrderStatusUseCase(
        IUnitOfWork unitOfWork,
        IOrderStatusRepository repository,
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
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.OrderStatusNotExist);

            _outputPort.NotFound();
        }
        else
        {
            if (result.Id == SystemConst.OrderStatusAvailableDefault ||
                result.Id == SystemConst.OrderStatusAcceptedDefault ||
                result.Id == SystemConst.OrderStatusDeliveredDefault ||
                result.Id == SystemConst.OrderStatusCancelledDefault)
            {
                _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

                _outputPort.Error();

                return;
            }


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
        IOutputPortWithNotFound<Domain.Entities.OrderStatus> outputPort)
        => _outputPort = outputPort;
}