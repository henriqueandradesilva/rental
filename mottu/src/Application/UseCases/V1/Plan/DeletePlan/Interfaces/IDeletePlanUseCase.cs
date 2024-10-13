using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Plan.DeletePlan.Interfaces;

public interface IDeletePlanUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Plan> outputPort);
}