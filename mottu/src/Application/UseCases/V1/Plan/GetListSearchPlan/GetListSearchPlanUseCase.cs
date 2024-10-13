using Application.UseCases.V1.Plan.GetListSearchPlan.Interfaces;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Application.UseCases.V1.Plan.GetListSearchPlan;

public class GetListSearchPlanUseCase : IGetListSearchPlanUseCase
{
    private IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Plan>> _outputPort;
    private readonly IPlanRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListSearchPlanUseCase(
        IPlanRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public void Execute(
        GenericSearchPaginationRequest genericSearchPaginationRequest)
    {
        var genericPaginationResponse = new GenericPaginationResponse<Domain.Entities.Plan>();

        var normalizedText = genericSearchPaginationRequest.Texto?.NormalizeString();

        var query =
            _repository?.Where(x => string.IsNullOrEmpty(normalizedText) ||
                                    (
                                     x.Description.ToUpper().Trim().Contains(normalizedText) ||
                                     x.PlanType.Description.ToUpper().Trim().Contains(normalizedText) ||
                                     x.DailyRate.ToString().ToUpper().Trim().Contains(normalizedText) ||
                                     x.AdditionalRate.ToString().ToUpper().Trim().Contains(normalizedText) ||
                                     x.DailyLateFee.ToString().ToUpper().Trim().Contains(normalizedText) ||
                                     x.DurationInDays.ToString().ToUpper().Trim().Contains(normalizedText)
                                    ) &&
                                    (genericSearchPaginationRequest.Ativo == null || x.IsActive == genericSearchPaginationRequest.Ativo));

        if (genericSearchPaginationRequest.Id.HasValue)
            query = _repository?.Where(x => x.Id == genericSearchPaginationRequest.Id.Value);

        query = DateFilterExtension<Domain.Entities.Plan>.Filter(genericSearchPaginationRequest, query);

        if (!string.IsNullOrEmpty(genericSearchPaginationRequest.CampoOrdenacao))
            query = query?.OrderBy($"{genericSearchPaginationRequest.CampoOrdenacao} {genericSearchPaginationRequest.DirecaoOrdenacao}");

        query = query?.Select(x => x);

        genericPaginationResponse.LoadPagination(query, genericSearchPaginationRequest);

        if (genericPaginationResponse == null || genericPaginationResponse.Total == 0)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.PlanTypeNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(genericPaginationResponse);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Plan>> outputPort)
        => _outputPort = outputPort;
}