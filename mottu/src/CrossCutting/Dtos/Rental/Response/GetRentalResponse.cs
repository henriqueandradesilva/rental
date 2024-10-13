using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.Driver.Response;
using CrossCutting.Dtos.Motorcycle.Response;
using CrossCutting.Dtos.Plan.Response;
using Domain.Common.Enums;
using System;

namespace CrossCutting.Dtos.Rental.Response;

public class GetRentalResponse : BaseResponse
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

    public GetRentalResponse()
    {

    }

    public GetRentalResponse GetRental(
        Domain.Entities.Rental rental)
    {
        if (rental == null)
            return null;

        GetRentalResponse getRentalResponse = new GetRentalResponse();
        getRentalResponse.Id = rental.Id;
        getRentalResponse.Moto_Id = rental.MotorcycleId;
        getRentalResponse.Entregador_Id = rental.DriverId;
        getRentalResponse.Plano_Id = rental.PlanId;
        getRentalResponse.Periodo_Alocado = rental.AllocatePeriod;
        getRentalResponse.Valor_Total = rental.TotalAmount;
        getRentalResponse.Data_Inicio = rental.StartDate;
        getRentalResponse.Data_Termino = rental.EndDate;
        getRentalResponse.Data_Previsao_Termino = rental.ExpectedEndDate;
        getRentalResponse.Status = rental.Status;
        getRentalResponse.Moto = new GetMotorcycleResponse().GetMotorcycle(rental.Motorcycle);
        getRentalResponse.Entregador = new GetDriverResponse().GetDriver(rental.Driver);
        getRentalResponse.Plano = new GetPlanResponse().GetPlan(rental.Plan);
        getRentalResponse.DataCriacao = rental.DateCreated;
        getRentalResponse.DataAlteracao = rental.DateUpdated;

        return getRentalResponse;
    }
}