using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Interfaces;

namespace Application.UseCases.V1.Driver.GetListSearchDriver.Interfaces;

public interface IGetListSearchDriverUseCase
{
    void Execute(
        GenericSearchPaginationRequest genericSearchPaginationRequest);

    void SetOutputPort(
        IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Driver>> outputPort);
}