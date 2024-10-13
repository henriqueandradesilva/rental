using CrossCutting.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Driver.GetListDriverByCnpj.Interfaces;

public interface IGetListDriverByCnpjUseCase
{
    Task Execute(
        string cnpj);

    void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.Driver>> outputPort);
}