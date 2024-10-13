using Application.UseCases.V1.PlanType.PutPlanType.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.PlanType.PutPlanType;

public class PutPlanTypeUseCase : IPutPlanTypeUseCase
{
    private IOutputPort<Domain.Entities.PlanType> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlanTypeRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PutPlanTypeUseCase(
        IUnitOfWork unitOfWork,
        IPlanTypeRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.PlanType planType)
    {
        var normalizedDescription = planType.Description?.NormalizeString();

        var result =
           await _repository?.Where(c => c.Id != planType.Id &&
                                         c.Description.ToUpper().Trim().Contains(normalizedDescription))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.PlanTypeExist);

            _outputPort.Error();
        }
        else
        {
            result =
                await _repository?.Where(c => c.Id == planType.Id)
                                 ?.FirstOrDefaultAsync();

            if (result == null)
            {
                _notificationHelper.Add(SystemConst.Error, MessageConst.PlanTypeNotExist);

                _outputPort.Error();
            }
            else
            {
                planType.Map(result);

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
        IOutputPort<Domain.Entities.PlanType> outputPort)
        => _outputPort = outputPort;
}