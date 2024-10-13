using CrossCutting.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Motorcycle.GetListAllMotorcycle.Interfaces;

public interface IGetListAllMotorcycleUseCase
{
    Task Execute();

    void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.Motorcycle>> outputPort);
}