using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.PlanType.DeletePlanType.Interfaces;

public interface IDeletePlanTypeUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.PlanType> outputPort);
}