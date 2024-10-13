using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.ModelVehicle.DeleteModelVehicle.Interfaces;

public interface IDeleteModelVehicleUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.ModelVehicle> outputPort);
}