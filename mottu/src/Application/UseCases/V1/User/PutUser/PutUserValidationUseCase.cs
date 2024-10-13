using Application.UseCases.V1.User.PutUser.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.V1.User.PutUser;

public class PutUserValidationUseCase : IPutUserUseCase
{
    private IOutputPort<Domain.Entities.User> _outputPort;
    private readonly IPutUserUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PutUserValidationUseCase(
        IPutUserUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.User user)
    {
        if (user.Invalid())
        {
            var listNotification = user.GetListNotification();

            foreach (var notification in listNotification)
                _notificationHelper.Add(SystemConst.Error, notification.ErrorMessage);

            _outputPort.Error();

            return;
        }

        try
        {
            await _useCase.Execute(user);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.User> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}