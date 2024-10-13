using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Plan.GetPlanById.Interfaces;

public interface IGetPlanByIdUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Plan> outputPort);
}