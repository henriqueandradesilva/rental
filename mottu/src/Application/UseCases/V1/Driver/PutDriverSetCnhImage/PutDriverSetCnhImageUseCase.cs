using Application.Services.Interfaces;
using Application.UseCases.V1.Driver.PutDriverSetCnhImage.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Driver.PutDriverSetCnhImage;

public class PutDriverSetCnhImageUseCase : IPutDriverSetCnhImageUseCase
{
    private IOutputPort<Domain.Entities.Driver> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDriverRepository _repository;
    private readonly NotificationHelper _notificationHelper;
    private readonly IFireBaseService _fireBaseService;

    public PutDriverSetCnhImageUseCase(
        IUnitOfWork unitOfWork,
        IDriverRepository repository,
        NotificationHelper notificationHelper,
        IFireBaseService fireBaseService)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _notificationHelper = notificationHelper;
        _fireBaseService = fireBaseService;
    }

    public async Task Execute(
        long id,
        string base64String)
    {
        var result =
                await _repository?.Where(c => c.Id == id)
                                 ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.DriverNotExist);

            _outputPort.Error();
        }
        else
        {
            var cnhImageUrl =
                await _fireBaseService.AddBucket("cnh")
                                      .UploadFileAsync(Convert.FromBase64String(base64String), Guid.NewGuid().ToString());

            result.SetCnhImageUrl(cnhImageUrl);

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
        IOutputPort<Domain.Entities.Driver> outputPort)
        => _outputPort = outputPort;
}