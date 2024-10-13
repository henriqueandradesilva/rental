using Application.UseCases.V1.Driver.PutDriverSetCnhImage.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Driver.PutDriverSetCnhImage;

public class PutDriverSetCnhImageValidationUseCase : IPutDriverSetCnhImageUseCase
{
    private IOutputPort<Domain.Entities.Driver> _outputPort;
    private readonly IPutDriverSetCnhImageUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PutDriverSetCnhImageValidationUseCase(
        IPutDriverSetCnhImageUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        long id,
        string cnhBase64)
    {
        if (!ImageExtension.IsPngOrBmp(cnhBase64))
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.DriverImageTypeInvalid);

            _outputPort.Error();

            return;
        }

        try
        {
            await _useCase.Execute(id, cnhBase64);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Driver> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}