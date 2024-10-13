using CrossCutting.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Driver.GetListAllDriver.Interfaces;

public interface IGetListAllDriverUseCase
{
    Task Execute();

    void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.Driver>> outputPort);
}