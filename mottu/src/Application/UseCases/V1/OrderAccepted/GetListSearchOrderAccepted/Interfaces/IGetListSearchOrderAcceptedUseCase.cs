using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Interfaces;

namespace Application.UseCases.V1.OrderAccepted.GetListSearchOrderAccepted.Interfaces;

public interface IGetListSearchOrderAcceptedUseCase
{
    void Execute(
        GenericSearchPaginationRequest genericSearchPaginationRequest);

    void SetOutputPort(
        IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.OrderAccepted>> outputPort);
}