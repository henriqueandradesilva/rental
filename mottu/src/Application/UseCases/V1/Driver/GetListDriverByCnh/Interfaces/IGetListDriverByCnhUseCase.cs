using CrossCutting.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Driver.GetListDriverByCnh.Interfaces;

public interface IGetListDriverByCnhUseCase
{
    Task Execute(
        string cnh);

    void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.Driver>> outputPort);
}