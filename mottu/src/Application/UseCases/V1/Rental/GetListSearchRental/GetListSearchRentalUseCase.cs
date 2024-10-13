using Application.UseCases.V1.Rental.GetListSearchRental.Interfaces;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Common.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Application.UseCases.V1.Rental.GetListSearchRental;

public class GetListSearchRentalUseCase : IGetListSearchRentalUseCase
{
    private IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Rental>> _outputPort;
    private IRentalRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListSearchRentalUseCase(
        IRentalRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public void Execute(
        GenericSearchPaginationRequest genericSearchPaginationRequest)
    {
        var genericPaginationResponse = new GenericPaginationResponse<Domain.Entities.Rental>();

        var normalizedText = genericSearchPaginationRequest.Texto?.NormalizeString();

        var query =
            _repository?.Where(x => string.IsNullOrEmpty(normalizedText) ||
                                    (
                                      x.Motorcycle.Identifier.ToUpper().Trim().Contains(normalizedText) ||
                                      x.Motorcycle.Year.ToString().ToUpper().Trim().Contains(normalizedText) ||
                                      x.Motorcycle.Plate.ToUpper().Trim().Contains(normalizedText) ||
                                      x.Driver.Identifier.ToUpper().Trim().Contains(normalizedText) ||
                                      x.Driver.Cnpj.ToUpper().Trim().Contains(normalizedText) ||
                                      x.Driver.Name.ToUpper().Trim().Contains(normalizedText) ||
                                      x.Driver.Cnh.ToUpper().Trim().Contains(normalizedText) ||
                                      x.Plan.Description.ToUpper().Trim().Contains(normalizedText)
                                    ));

        if (genericSearchPaginationRequest.Id.HasValue)
            query = _repository?.Where(x => x.Id == genericSearchPaginationRequest.Id.Value);

        if (genericSearchPaginationRequest.ListaRelacionamento != null &&
            genericSearchPaginationRequest.ListaRelacionamento.Any())
        {
            foreach (var relational in genericSearchPaginationRequest.ListaRelacionamento)
            {
                if (relational.Item1.ToUpper() == SystemConst.FieldMotorcycleId.ToUpper())
                    query = query.Where(c => c.MotorcycleId == relational.Item2);

                if (relational.Item1.ToUpper() == SystemConst.FieldDriverId.ToUpper())
                    query = query.Where(c => c.DriverId == relational.Item2);

                if (relational.Item1.ToUpper() == SystemConst.FieldPlanId.ToUpper())
                    query = query.Where(c => c.PlanId == relational.Item2);
            }
        }

        if (genericSearchPaginationRequest.ListaEnum != null &&
            genericSearchPaginationRequest.ListaEnum.Any())
        {
            foreach (var enumerator in genericSearchPaginationRequest.ListaEnum)
            {
                if (enumerator.Item1.ToUpper() == SystemConst.FieldRentalStatus.ToUpper())
                    query = query.Where(c => c.Status == (RentalStatusEnum)enumerator.Item2);
            }
        }

        query =
            query.Where(x => (!genericSearchPaginationRequest.DataInicio.HasValue || (x.StartDate >= genericSearchPaginationRequest.DataInicio.Value ||
                                                                                      x.EndDate >= genericSearchPaginationRequest.DataInicio)) &&
                             (!genericSearchPaginationRequest.DataFim.HasValue || (x.StartDate <= genericSearchPaginationRequest.DataFim.Value ||
                                                                                   x.EndDate <= genericSearchPaginationRequest.DataFim.Value)));

        if (!string.IsNullOrEmpty(genericSearchPaginationRequest.CampoOrdenacao))
            query = query?.OrderBy($"{genericSearchPaginationRequest.CampoOrdenacao} {genericSearchPaginationRequest.DirecaoOrdenacao}");

        query = query?.Include(c => c.Motorcycle).ThenInclude(c => c.ModelVehicle)
                     ?.Include(c => c.Driver).ThenInclude(c => c.User)
                     ?.Include(c => c.Plan).ThenInclude(c => c.PlanType)
                     ?.Select(x => x);

        genericPaginationResponse.LoadPagination(query, genericSearchPaginationRequest);

        if (genericPaginationResponse == null || genericPaginationResponse.Total == 0)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.RentalNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(genericPaginationResponse);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Rental>> outputPort)
        => _outputPort = outputPort;
}