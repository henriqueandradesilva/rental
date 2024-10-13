using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.User.PostUser.Interfaces;

public interface IPostUserUseCase
{
    Task Execute(
        Domain.Entities.User user);

    void SetOutputPort(
        IOutputPort<Domain.Entities.User> outputPort);
}