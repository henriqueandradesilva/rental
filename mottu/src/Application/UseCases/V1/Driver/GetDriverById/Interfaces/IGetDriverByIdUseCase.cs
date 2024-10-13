using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Driver.GetDriverById.Interfaces;

public interface IGetDriverByIdUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Driver> outputPort);
}