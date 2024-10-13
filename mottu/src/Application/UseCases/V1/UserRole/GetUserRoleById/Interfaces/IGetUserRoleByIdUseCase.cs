using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.UserRole.GetUserRoleById.Interfaces;

public interface IGetUserRoleByIdUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.UserRole> outputPort);
}