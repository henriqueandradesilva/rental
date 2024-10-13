using Application.Services.Interfaces;
using Application.UseCases.V1.OrderAccepted.PutOrderAccepted.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderAccepted.PutOrderAccepted;

public class PutOrderAcceptedUseCase : IPutOrderAcceptedUseCase
{
    private IOutputPort<Domain.Entities.OrderAccepted> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderAcceptedRepository _repository;
    private readonly IOrderAcceptedService _orderAcceptedService;
    private readonly NotificationHelper _notificationHelper;

    public PutOrderAcceptedUseCase(
        IUnitOfWork unitOfWork,
        IOrderAcceptedRepository repository,
        IOrderAcceptedService orderAcceptedService,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _orderAcceptedService = orderAcceptedService;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.OrderAccepted orderAccepted)
    {
        var result =
           await _repository?.Where(c => c.Id != orderAccepted.Id &&
                                         c.DriverId == orderAccepted.DriverId &&
                                         c.OrderId == orderAccepted.OrderId)
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.OrderAcceptedExist);

            _outputPort.Error();
        }
        else
        {
            result =
                await _repository?.Where(c => c.Id == orderAccepted.Id)
                                 ?.FirstOrDefaultAsync();

            if (result == null)
            {
                _notificationHelper.Add(SystemConst.Error, MessageConst.OrderAcceptedNotExist);

                _outputPort.Error();
            }
            else
            {
                var order =
                    await _orderAcceptedService.InitOrder(_outputPort,
                                                          _notificationHelper,
                                                          orderAccepted.OrderId);

                if (!order)
                    return;

                var delivering =
                    await _orderAcceptedService.InitDriver(_outputPort,
                                                           _notificationHelper,
                                                           orderAccepted.DriverId);

                if (!delivering)
                    return;

                orderAccepted.Map(result);

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
        IOutputPort<Domain.Entities.OrderAccepted> outputPort)
        => _outputPort = outputPort;
}