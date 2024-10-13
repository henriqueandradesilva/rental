using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.PlanType.PutPlanType.Interfaces;

public interface IPutPlanTypeUseCase
{
    Task Execute(
        Domain.Entities.PlanType planType);

    void SetOutputPort(
        IOutputPort<Domain.Entities.PlanType> outputPort);
}