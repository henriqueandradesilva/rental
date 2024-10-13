using CrossCutting.Common.Dtos.Response;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Dtos.PlanType.Response;

public class GetListPlanTypeResponse : BaseResponse
{
    public string Descricao { get; set; }

    public GetListPlanTypeResponse()
    {

    }

    public List<GetListPlanTypeResponse> GetListPlanType(
        List<Domain.Entities.PlanType> listPlanType)
    {
        if (listPlanType == null)
            return null;

        return listPlanType
        .Select(e => new GetListPlanTypeResponse()
        {
            Id = e.Id,
            Descricao = e.Description,
            DataCriacao = e.DateCreated,
            DataAlteracao = e.DateUpdated
        })
        .ToList();
    }
}