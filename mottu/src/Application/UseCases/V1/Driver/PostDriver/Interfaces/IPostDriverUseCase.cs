using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Driver.PostDriver.Interfaces;

public interface IPostDriverUseCase
{
    Task Execute(
        Domain.Entities.Driver driver,
        string cnhBase64);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Driver> outputPort);
}