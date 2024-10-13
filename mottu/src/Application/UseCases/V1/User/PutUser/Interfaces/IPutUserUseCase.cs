using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.User.PutUser.Interfaces;

public interface IPutUserUseCase
{
    Task Execute(
        Domain.Entities.User user);

    void SetOutputPort(
        IOutputPort<Domain.Entities.User> outputPort);
}