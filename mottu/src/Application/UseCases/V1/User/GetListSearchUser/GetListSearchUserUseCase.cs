using Application.UseCases.V1.User.GetListSearchUser.Interfaces;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Application.UseCases.V1.User.GetListSearchUser;

public class GetListSearchUserUseCase : IGetListSearchUserUseCase
{
    private IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.User>> _outputPort;
    private readonly IUserRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListSearchUserUseCase(
        IUserRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public void Execute(
        GenericSearchPaginationRequest genericSearchPaginationRequest)
    {
        var genericPaginationResponse = new GenericPaginationResponse<Domain.Entities.User>();

        var normalizedText = genericSearchPaginationRequest.Texto?.NormalizeString();

        var query =
            _repository?.Where(x => string.IsNullOrEmpty(normalizedText) ||
                                    (
                                     x.Name.ToUpper().Trim().Contains(normalizedText) ||
                                     x.Driver.Cnpj.ToUpper().Trim().Contains(normalizedText) ||
                                     x.Driver.Cnh.ToUpper().Trim().Contains(normalizedText) ||
                                     x.Driver.User.Name.ToUpper().Trim().Contains(normalizedText) ||
                                     x.Driver.User.Email.ToUpper().Trim().Contains(normalizedText)
                                    ) &&
                                    (genericSearchPaginationRequest.Ativo == null || x.IsActive == genericSearchPaginationRequest.Ativo));

        if (genericSearchPaginationRequest.Id.HasValue)
            query = _repository?.Where(x => x.Id == genericSearchPaginationRequest.Id.Value);

        if (genericSearchPaginationRequest.ListaRelacionamento != null &&
            genericSearchPaginationRequest.ListaRelacionamento.Any())
        {
            foreach (var relational in genericSearchPaginationRequest.ListaRelacionamento)
            {
                if (relational.Item1.ToUpper() == SystemConst.FieldUserRoleId.ToUpper())
                    query = query.Where(c => c.UserRoleId == relational.Item2);

                if (relational.Item1.ToUpper() == SystemConst.FieldDriverId.ToUpper())
                    query = query.Where(c => c.Driver.Id == relational.Item2);
            }
        }

        query = DateFilterExtension<Domain.Entities.User>.Filter(genericSearchPaginationRequest, query);

        if (!string.IsNullOrEmpty(genericSearchPaginationRequest.CampoOrdenacao))
            query = query?.OrderBy($"{genericSearchPaginationRequest.CampoOrdenacao} {genericSearchPaginationRequest.DirecaoOrdenacao}");

        query = query?.Include(c => c.UserRole)
                     ?.Include(c => c.Driver)
                     ?.Select(x => x);

        genericPaginationResponse.LoadPagination(query, genericSearchPaginationRequest);

        if (genericPaginationResponse == null || genericPaginationResponse.Total == 0)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.UserNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(genericPaginationResponse);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.User>> outputPort)
        => _outputPort = outputPort;
}