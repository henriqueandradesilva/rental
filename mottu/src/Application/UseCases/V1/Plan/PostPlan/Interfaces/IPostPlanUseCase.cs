using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Plan.PostPlan.Interfaces;

public interface IPostPlanUseCase
{
    Task Execute(
        Domain.Entities.Plan plan);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Plan> outputPort);
}