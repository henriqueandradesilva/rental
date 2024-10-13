using CrossCutting.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UseCases.V1.UserRole.GetListAllUserRole.Interfaces;

public interface IGetListAllUserRoleUseCase
{
    Task Execute();

    void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.UserRole>> outputPort);
}