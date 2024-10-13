using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.User.PutUserSetActive.Interfaces;

public interface IPutUserSetActiveUseCase
{
    Task Execute(
        long id,
        bool active);

    void SetOutputPort(
        IOutputPort<Domain.Entities.User> outputPort);
}