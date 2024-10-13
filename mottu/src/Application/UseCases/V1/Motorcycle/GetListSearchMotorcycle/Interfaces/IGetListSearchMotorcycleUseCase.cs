using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Interfaces;

namespace Application.UseCases.V1.Motorcycle.GetListSearchMotorcycle.Interfaces;

public interface IGetListSearchMotorcycleUseCase
{
    void Execute(
        GenericSearchPaginationRequest genericSearchPaginationRequest);

    void SetOutputPort(
        IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Motorcycle>> outputPort);
}