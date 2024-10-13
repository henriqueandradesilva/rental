using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.UserRole.DeleteUserRole.Interfaces;

public interface IDeleteUserRoleUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.UserRole> outputPort);
}