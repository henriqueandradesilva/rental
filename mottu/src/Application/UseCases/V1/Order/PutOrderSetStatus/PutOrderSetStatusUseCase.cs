using Application.UseCases.V1.Order.PutOrderSetStatus.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Order.PutOrderSetStatus;

public class PutOrderSetStatusUseCase : IPutOrderSetStatusUseCase
{
    private IOutputPort<Domain.Entities.Order> _outputPort;
    private IUnitOfWork _unitOfWork;
    private IOrderRepository _repository;
    private IOrderAcceptedRepository _orderAcceptedRepository;
    private IOrderDeliveredRepository _orderDeliveredRepository;
    private readonly NotificationHelper _notificationHelper;

    public PutOrderSetStatusUseCase(
        IUnitOfWork unitOfWork,
        IOrderRepository repository,
        IOrderAcceptedRepository orderAcceptedRepository,
        IOrderDeliveredRepository orderDeliveredRepository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _orderAcceptedRepository = orderAcceptedRepository;
        _orderDeliveredRepository = orderDeliveredRepository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        long id,
        long orderStatusId)
    {
        var result =
                await _repository?.Where(c => c.Id == id)
                                 ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.OrderNotExist);

            _outputPort.Error();
        }
        else
        {
            if (orderStatusId == SystemConst.OrderStatusCancelledDefault)
            {
                var orderAccepted =
                    _orderAcceptedRepository?.Where(c => c.OrderId == id)
                                            ?.FirstOrDefault();

                if (orderAccepted != null)
                    _orderAcceptedRepository.Delete(orderAccepted);

                var orderDelivered =
                    _orderDeliveredRepository?.Where(c => c.OrderId == id)
                                             ?.FirstOrDefault();

                if (orderDelivered != null)
                    _orderDeliveredRepository.Delete(orderDelivered);
            }

            result.SetDateUpdated();

            result.SetOrderStatus(orderStatusId);

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

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Order> outputPort)
        => _outputPort = outputPort;
}