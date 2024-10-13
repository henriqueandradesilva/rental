using CrossCutting.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UseCases.V1.PlanType.GetListAllPlanType.Interfaces;

public interface IGetListAllPlanTypeUseCase
{
    Task Execute();

    void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.PlanType>> outputPort);
}