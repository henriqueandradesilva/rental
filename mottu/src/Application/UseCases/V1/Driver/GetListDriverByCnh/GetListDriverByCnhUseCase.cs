using Application.UseCases.V1.Driver.GetListDriverByCnh.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Driver.GetListDriverByCnh;

public class GetListDriverByCnhUseCase : IGetListDriverByCnhUseCase
{
    private IOutputPortWithNotFound<List<Domain.Entities.Driver>> _outputPort;
    private readonly IDriverRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListDriverByCnhUseCase(
        IDriverRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        string cnh)
    {
        var normalizedCnh = cnh?.NormalizeString();

        var result =
            await _repository?.Where(c => c.Cnh.ToUpper().Trim().Contains(normalizedCnh))
                             ?.Include(c => c.User)
                             ?.ToListAsync();

        if (result == null || !result.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.DriverNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.Driver>> outputPort)
        => _outputPort = outputPort;
}