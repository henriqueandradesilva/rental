using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Plan.PutPlan.Interfaces;

public interface IPutPlanUseCase
{
    Task Execute(
        Domain.Entities.Plan plan);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Plan> outputPort);
}