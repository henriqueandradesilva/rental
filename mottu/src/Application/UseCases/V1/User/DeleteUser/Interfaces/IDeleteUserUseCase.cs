using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.User.DeleteUser.Interfaces;

public interface IDeleteUserUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.User> outputPort);
}