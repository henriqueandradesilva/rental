using Application.Services.Interfaces;
using Application.UseCases.V1.User.PostUser.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.V1.User.PostUser;

public class PostUserValidationUseCase : IPostUserUseCase
{
    private IOutputPort<Domain.Entities.User> _outputPort;
    private readonly IPostUserUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;
    private readonly IFireBaseService _fireBaseService;

    public PostUserValidationUseCase(
        IPostUserUseCase useCase,
        NotificationHelper notificationHelper,
        IFireBaseService fireBaseService)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
        _fireBaseService = fireBaseService;
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