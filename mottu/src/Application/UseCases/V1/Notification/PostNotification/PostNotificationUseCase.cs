using Application.UseCases.V1.Notification.PostNotification.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Notification.PostNotification;

public class PostNotificationUseCase : IPostNotificationUseCase
{
    private IOutputPort<Domain.Entities.Notification> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PostNotificationUseCase(
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
           await _repository?.Where(c => c.OrderId == notification.OrderId &&
                                         c.Date == notification.Date)
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.NotificationExist);

            _outputPort.Error();
        }
        else if (notification.Id == 0)
        {
            notification.SetDateCreated();

            await _repository.Add(notification)
                             .ConfigureAwait(false);

            var response =
                await _unitOfWork.Save()
                                 .ConfigureAwait(false);

            if (!string.IsNullOrEmpty(response))
            {
                _notificationHelper.Add(SystemConst.Error, response);

                _outputPort.Error();
            }
            else
                _outputPort.Ok(notification);
        }
        else
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Notification> outputPort)
        => _outputPort = outputPort;
}