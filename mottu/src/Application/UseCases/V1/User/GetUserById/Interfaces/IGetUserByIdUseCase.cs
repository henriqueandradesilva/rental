using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.User.GetUserById.Interfaces;

public interface IGetUserByIdUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.User> outputPort);
}