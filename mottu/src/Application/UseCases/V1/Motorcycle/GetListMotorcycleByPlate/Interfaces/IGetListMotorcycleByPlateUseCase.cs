using CrossCutting.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Motorcycle.GetListMotorcycleByPlate.Interfaces;

public interface IGetListMotorcycleByPlateUseCase
{
    Task Execute(
        string plate);

    void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.Motorcycle>> outputPort);
}