using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.UserRole.PostUserRole.Interfaces;

public interface IPostUserRoleUseCase
{
    Task Execute(
        Domain.Entities.UserRole userRole);

    void SetOutputPort(
        IOutputPort<Domain.Entities.UserRole> outputPort);
}