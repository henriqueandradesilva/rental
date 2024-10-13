using Application.Services.Interfaces;
using Application.UseCases.V1.OrderAccepted.PostOrderAccepted.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderAccepted.PostOrderAccepted;

public class PostOrderAcceptedUseCase : IPostOrderAcceptedUseCase
{
    private IOutputPort<Domain.Entities.OrderAccepted> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderAcceptedRepository _repository;
    private readonly IOrderAcceptedService _orderAcceptedService;
    private readonly NotificationHelper _notificationHelper;

    public PostOrderAcceptedUseCase(
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
           await _repository?.Where(c => c.DriverId == orderAccepted.DriverId &&
                                         c.OrderId == orderAccepted.OrderId)
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.OrderAcceptedExist);

            _outputPort.Error();
        }
        else if (orderAccepted.Id == 0)
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

            orderAccepted.SetDateCreated();

            await _repository.Add(orderAccepted)
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
                _outputPort.Ok(orderAccepted);
        }
        else
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.OrderAccepted> outputPort)
        => _outputPort = outputPort;
}