using Application.Services.Interfaces;
using Application.UseCases.V1.OrderDelivered.PostOrderDelivered.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderDelivered.PostOrderDelivered;

public class PostOrderDeliveredUseCase : IPostOrderDeliveredUseCase
{
    private IOutputPort<Domain.Entities.OrderDelivered> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderDeliveredRepository _repository;
    private readonly IOrderDeliveredService _orderDeliveredService;
    private readonly NotificationHelper _notificationHelper;

    public PostOrderDeliveredUseCase(
        IUnitOfWork unitOfWork,
        IOrderDeliveredRepository repository,
        IOrderDeliveredService orderDeliveredService,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _orderDeliveredService = orderDeliveredService;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.OrderDelivered orderDelivered)
    {
        var result =
           await _repository?.Where(c => c.DriverId == orderDelivered.DriverId &&
                                         c.OrderId == orderDelivered.OrderId)
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.OrderDeliveredExist);

            _outputPort.Error();
        }
        else if (orderDelivered.Id == 0)
        {
            var order =
                await _orderDeliveredService.InitOrder(_outputPort,
                                                       _notificationHelper,
                                                       orderDelivered.OrderId);

            if (!order)
                return;

            var delivering =
                await _orderDeliveredService.InitDriver(_outputPort,
                                                        _notificationHelper,
                                                        orderDelivered.DriverId);

            if (!delivering)
                return;

            orderDelivered.SetDateCreated();

            await _repository.Add(orderDelivered)
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
                _outputPort.Ok(orderDelivered);
        }
        else
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.OrderDelivered> outputPort)
        => _outputPort = outputPort;
}