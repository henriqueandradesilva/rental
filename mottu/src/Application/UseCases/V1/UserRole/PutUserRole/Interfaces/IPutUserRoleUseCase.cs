using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.UserRole.PutUserRole.Interfaces;

public interface IPutUserRoleUseCase
{
    Task Execute(
        Domain.Entities.UserRole userRole);

    void SetOutputPort(
        IOutputPort<Domain.Entities.UserRole> outputPort);
}