using CrossCutting.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UseCases.V1.ModelVehicle.GetListAllModelVehicle.Interfaces;

public interface IGetListAllModelVehicleUseCase
{
    Task Execute();

    void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.ModelVehicle>> outputPort);
}