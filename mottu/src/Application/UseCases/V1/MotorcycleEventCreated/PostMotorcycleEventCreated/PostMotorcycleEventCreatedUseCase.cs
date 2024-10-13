using Application.UseCases.V1.MotorcycleEventCreated.PostMotorcycleEventCreated.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using System.Threading.Tasks;

namespace Application.UseCases.V1.MotorcycleEventCreated.PostMotorcycleEventCreated;

public class PostMotorcycleEventCreatedUseCase : IPostMotorcycleEventCreatedUseCase
{
    private IOutputPort<Domain.Entities.MotorcycleEventCreated> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMotorcycleEventCreatedRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PostMotorcycleEventCreatedUseCase(
        IUnitOfWork unitOfWork,
        IMotorcycleEventCreatedRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.MotorcycleEventCreated motorcycleEventCreated)
    {
        if (motorcycleEventCreated.Id == 0)
        {
            motorcycleEventCreated.SetDateCreated();

            await _repository.Add(motorcycleEventCreated)
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
                _outputPort.Ok(motorcycleEventCreated);
        }
        else
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.MotorcycleEventCreated> outputPort)
        => _outputPort = outputPort;
}