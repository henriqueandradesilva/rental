using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.PlanType.GetPlanTypeById.Interfaces;

public interface IGetPlanTypeByIdUseCase
{
    Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.PlanType> outputPort);
}