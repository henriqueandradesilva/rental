using Application.UseCases.V1.OrderStatus.PutOrderStatus.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderStatus.PutOrderStatus;

public class PutOrderStatusUseCase : IPutOrderStatusUseCase
{
    private IOutputPort<Domain.Entities.OrderStatus> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderStatusRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PutOrderStatusUseCase(
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
           await _repository?.Where(c => c.Id != orderStatus.Id &&
                                         c.Description.ToUpper().Trim().Contains(normalizedDescription))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.OrderStatusExist);

            _outputPort.Error();
        }
        else
        {
            result =
                await _repository?.Where(c => c.Id == orderStatus.Id)
                                 ?.FirstOrDefaultAsync();

            if (result == null)
            {
                _notificationHelper.Add(SystemConst.Error, MessageConst.OrderStatusNotExist);

                _outputPort.Error();
            }
            else
            {
                orderStatus.Map(result);

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
        IOutputPort<Domain.Entities.OrderStatus> outputPort)
        => _outputPort = outputPort;
}