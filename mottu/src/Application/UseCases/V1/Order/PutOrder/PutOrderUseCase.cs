using Application.UseCases.V1.Order.PutOrder.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Order.PutOrder;

public class PutOrderUseCase : IPutOrderUseCase
{
    private IOutputPort<Domain.Entities.Order> _outputPort;
    private IUnitOfWork _unitOfWork;
    private IOrderRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PutOrderUseCase(
        IUnitOfWork unitOfWork,
        IOrderRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.Order order)
    {
        var normalizedDescription = order.Description?.NormalizeString();

        var result =
           await _repository?.Where(c => c.Id != order.Id &&
                                         c.Description.ToUpper().Trim().Contains(normalizedDescription))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.OrderExist);

            _outputPort.Error();
        }
        else
        {
            result =
                await _repository?.Where(c => c.Id == order.Id)
                                 ?.FirstOrDefaultAsync();

            if (result == null)
            {
                _notificationHelper.Add(SystemConst.Error, MessageConst.OrderNotExist);

                _outputPort.Error();
            }
            else
            {
                order.Map(result);

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
        IOutputPort<Domain.Entities.Order> outputPort)
        => _outputPort = outputPort;
}