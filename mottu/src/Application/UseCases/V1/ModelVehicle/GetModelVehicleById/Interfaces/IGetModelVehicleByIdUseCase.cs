using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.ModelVehicle.GetModelVehicleById.Interfaces;

public interface IGetModelVehicleByIdUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.ModelVehicle> outputPort);
}