using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Interfaces;

namespace Application.UseCases.V1.UserRole.GetListSearchUserRole.Interfaces;

public interface IGetListSearchUserRoleUseCase
{
    void Execute(
        GenericSearchPaginationRequest genericSearchPaginationRequest);

    void SetOutputPort(
        IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.UserRole>> outputPort);
}