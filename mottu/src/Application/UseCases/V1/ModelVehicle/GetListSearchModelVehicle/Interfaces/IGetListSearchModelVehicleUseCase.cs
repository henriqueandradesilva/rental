using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Interfaces;

namespace Application.UseCases.V1.ModelVehicle.GetListSearchModelVehicle.Interfaces;

public interface IGetListSearchModelVehicleUseCase
{
    void Execute(
        GenericSearchPaginationRequest genericSearchPaginationRequest);

    void SetOutputPort(
        IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.ModelVehicle>> outputPort);
}