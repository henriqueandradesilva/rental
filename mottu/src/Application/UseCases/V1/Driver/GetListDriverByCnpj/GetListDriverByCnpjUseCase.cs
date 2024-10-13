using Application.UseCases.V1.Driver.GetListDriverByCnpj.Interfaces;
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

namespace Application.UseCases.V1.Driver.GetListDriverByCnpj;

public class GetListDriverByCnpjUseCase : IGetListDriverByCnpjUseCase
{
    private IOutputPortWithNotFound<List<Domain.Entities.Driver>> _outputPort;
    private readonly IDriverRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListDriverByCnpjUseCase(
        IDriverRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        string cnpj)
    {
        var normalizedCnpj = cnpj?.NormalizeString();

        var result =
            await _repository?.Where(c => c.Cnpj.ToUpper().Trim().Contains(normalizedCnpj))
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