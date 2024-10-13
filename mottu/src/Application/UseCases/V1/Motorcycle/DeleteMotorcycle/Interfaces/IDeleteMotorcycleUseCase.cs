using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Motorcycle.DeleteMotorcycle.Interfaces;

public interface IDeleteMotorcycleUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Motorcycle> outputPort);
}