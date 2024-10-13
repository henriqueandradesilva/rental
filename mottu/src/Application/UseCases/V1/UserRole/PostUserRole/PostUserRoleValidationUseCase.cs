using Application.UseCases.V1.UserRole.PostUserRole.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.V1.UserRole.PostUserRole;

public class PostUserRoleValidationUseCase : IPostUserRoleUseCase
{
    private IOutputPort<Domain.Entities.UserRole> _outputPort;
    private readonly IPostUserRoleUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PostUserRoleValidationUseCase(
        IPostUserRoleUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.UserRole userRole)
    {
        if (userRole.Invalid())
        {
            var listNotification = userRole.GetListNotification();

            foreach (var notification in listNotification)
                _notificationHelper.Add(SystemConst.Error, notification.ErrorMessage);

            _outputPort.Error();

            return;
        }

        try
        {
            await _useCase.Execute(userRole);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.UserRole> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}