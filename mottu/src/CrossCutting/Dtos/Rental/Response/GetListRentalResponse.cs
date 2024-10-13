using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.Driver.Response;
using CrossCutting.Dtos.Motorcycle.Response;
using CrossCutting.Dtos.Plan.Response;
using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Dtos.Rental.Response;

public class GetListRentalResponse : BaseResponse
{
    public long Moto_Id { get; set; }

    public long Entregador_Id { get; set; }

    public long Plano_Id { get; set; }

    public int Periodo_Alocado { get; set; }

    public double Valor_Total { get; set; }

    public DateTime Data_Inicio { get; set; }

    public DateTime? Data_Termino { get; set; }

    public DateTime Data_Previsao_Termino { get; set; }

    public RentalStatusEnum Status { get; set; }

    public GetMotorcycleResponse Moto { get; set; }

    public GetDriverResponse Entregador { get; set; }

    public GetPlanResponse Plano { get; set; }

    public GetListRentalResponse()
    {

    }

    public List<GetListRentalResponse> GetListRental(
        List<Domain.Entities.Rental> listRental)
    {
        if (listRental == null)
            return null;

        return listRental
        .Select(e => new GetListRentalResponse()
        {
            Id = e.Id,
            Moto_Id = e.MotorcycleId,
            Entregador_Id = e.DriverId,
            Plano_Id = e.PlanId,
            Periodo_Alocado = e.AllocatePeriod,
            Valor_Total = e.TotalAmount,
            Data_Inicio = e.StartDate,
            Data_Termino = e.EndDate,
            Data_Previsao_Termino = e.ExpectedEndDate,
            Status = e.Status,
            Moto = new GetMotorcycleResponse().GetMotorcycle(e.Motorcycle),
            Entregador = new GetDriverResponse().GetDriver(e.Driver),
            Plano = new GetPlanResponse().GetPlan(e.Plan),
            DataCriacao = e.DateCreated,
            DataAlteracao = e.DateUpdated
        })
        .ToList();
    }
}