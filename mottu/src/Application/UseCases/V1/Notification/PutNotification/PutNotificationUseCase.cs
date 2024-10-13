using Application.UseCases.V1.Notification.PutNotification.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Notification.PutNotification;

public class PutNotificationUseCase : IPutNotificationUseCase
{
    private IOutputPort<Domain.Entities.Notification> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PutNotificationUseCase(
        IUnitOfWork unitOfWork,
        INotificationRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.Notification notification)
    {
        var result =
           await _repository?.Where(c => c.Id != notification.Id &&
                                         c.OrderId == notification.OrderId &&
                                         c.Date == notification.Date)
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.NotificationExist);

            _outputPort.Error();
        }
        else
        {
            result =
                await _repository?.Where(c => c.Id == notification.Id)
                                 ?.FirstOrDefaultAsync();

            if (result == null)
            {
                _notificationHelper.Add(SystemConst.Error, MessageConst.NotificationNotExist);

                _outputPort.Error();
            }
            else
            {
                notification.Map(result);

                result.SetDateUpdated();

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
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Notification> outputPort)
        => _outputPort = outputPort;
}