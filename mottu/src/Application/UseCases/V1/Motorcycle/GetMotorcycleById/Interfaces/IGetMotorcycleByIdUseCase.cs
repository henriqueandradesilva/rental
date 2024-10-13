using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Motorcycle.GetMotorcycleById.Interfaces;

public interface IGetMotorcycleByIdUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Motorcycle> outputPort);
}