using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Plan.PutPlanSetActive.Interfaces;

public interface IPutPlanSetActiveUseCase
{
    Task Execute(
        long id,
        bool active);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Plan> outputPort);
}