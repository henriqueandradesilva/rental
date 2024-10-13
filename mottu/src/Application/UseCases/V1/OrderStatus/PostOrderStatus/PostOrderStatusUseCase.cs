using Application.UseCases.V1.OrderStatus.PostOrderStatus.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderStatus.PostOrderStatus;

public class PostOrderStatusUseCase : IPostOrderStatusUseCase
{
    private IOutputPort<Domain.Entities.OrderStatus> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderStatusRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PostOrderStatusUseCase(
        IUnitOfWork unitOfWork,
        IOrderStatusRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.OrderStatus orderStatus)
    {
        var normalizedDescription = orderStatus.Description?.NormalizeString();

        var result =
           await _repository?.Where(c => c.Description.ToUpper().Trim().Contains(normalizedDescription))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.OrderStatusExist);

            _outputPort.Error();
        }
        else if (orderStatus.Id == 0)
        {
            orderStatus.SetDateCreated();

            await _repository.Add(orderStatus)
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
                _outputPort.Ok(orderStatus);
        }
        else
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.OrderStatus> outputPort)
        => _outputPort = outputPort;
}