using Application.Services.Interfaces;
using Application.UseCases.V1.Order.PostOrder.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Order.PostOrder;

public class PostOrderUseCase : IPostOrderUseCase
{
    private IOutputPort<Domain.Entities.Order> _outputPort;
    private IUnitOfWork _unitOfWork;
    private IOrderRepository _repository;
    private readonly IRabbitMqService _rabbitMqService;
    private readonly NotificationHelper _notificationHelper;

    public PostOrderUseCase(
        IUnitOfWork unitOfWork,
        IOrderRepository repository,
        IRabbitMqService rabbitMqService,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _rabbitMqService = rabbitMqService;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.Order order)
    {
        var normalizedDescription = order.Description?.NormalizeString();

        var result =
           await _repository?.Where(c => c.Description.ToUpper().Trim().Contains(normalizedDescription))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.OrderExist);

            _outputPort.Error();
        }
        else if (order.Id == 0)
        {
            order.SetDateCreated();

            await _repository.Add(order)
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
            {
                _rabbitMqService.SendMessage(order, SystemConst.OrderEventCreatedQueue);

                _outputPort.Ok(order);
            }
        }
        else
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Order> outputPort)
        => _outputPort = outputPort;
}