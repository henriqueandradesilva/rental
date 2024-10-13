using Application.UseCases.V1.Motorcycle.GetListSearchMotorcycle.Interfaces;
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

namespace Application.UseCases.V1.Motorcycle.GetListSearchMotorcycle;

public class GetListSearchMotorcycleUseCase : IGetListSearchMotorcycleUseCase
{
    private IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Motorcycle>> _outputPort;
    private readonly IMotorcycleRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListSearchMotorcycleUseCase(
        IMotorcycleRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public void Execute(
        GenericSearchPaginationRequest genericSearchPaginationRequest)
    {
        var genericPaginationResponse = new GenericPaginationResponse<Domain.Entities.Motorcycle>();

        var normalizedText = genericSearchPaginationRequest.Texto?.NormalizeString();

        var query =
            _repository?.Where(x => (string.IsNullOrEmpty(normalizedText) ||
                                    (
                                        x.Identifier.ToUpper().Trim().Contains(normalizedText) ||
                                        x.Year.ToString().Trim().Contains(normalizedText) ||
                                        x.ModelVehicle.Description.ToUpper().Trim().Contains(normalizedText) ||
                                        x.Plate.ToUpper().Trim().Contains(normalizedText)
                                      )
                                    ) &&
                                    (genericSearchPaginationRequest.Alugado == null || x.IsRented == genericSearchPaginationRequest.Alugado));

        if (genericSearchPaginationRequest.Id.HasValue)
            query = _repository?.Where(x => x.Id == genericSearchPaginationRequest.Id.Value);

        if (genericSearchPaginationRequest.ListaRelacionamento != null &&
            genericSearchPaginationRequest.ListaRelacionamento.Any())
        {
            foreach (var relational in genericSearchPaginationRequest.ListaRelacionamento)
            {
                if (relational.Item1.ToUpper() == SystemConst.FieldModelVehicleId.ToUpper())
                    query = query.Where(c => c.ModelVehicleId == relational.Item2);
            }
        }

        query = DateFilterExtension<Domain.Entities.Motorcycle>.Filter(genericSearchPaginationRequest, query);

        if (!string.IsNullOrEmpty(genericSearchPaginationRequest.CampoOrdenacao))
            query = query?.OrderBy($"{genericSearchPaginationRequest.CampoOrdenacao} {genericSearchPaginationRequest.DirecaoOrdenacao}");

        query = query?.Include(c => c.ModelVehicle)
                     ?.Select(x => x);

        genericPaginationResponse.LoadPagination(query, genericSearchPaginationRequest);

        if (genericPaginationResponse == null || genericPaginationResponse.Total == 0)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.MotorcycleNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(genericPaginationResponse);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Motorcycle>> outputPort)
        => _outputPort = outputPort;
}