using CrossCutting.Common.Dtos.Request;
using CrossCutting.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Common.Dtos.Response;

public class GenericPaginationResponse<T> where T : new()
{
    public bool Sucesso { get; set; }

    public long Total { get; set; }

    public List<T> ListaResultado { get; set; }

    public List<GenericNotificationResponse> ListaNotificacao { get; set; }

    public GenericPaginationResponse()
    {

    }

    public GenericPaginationResponse(
        bool success,
        List<T> listResult,
        long total,
        List<string> listNotification,
        NotificationTypeEnum notificatioNType)
    {
        Sucesso = success;
        ListaResultado = listResult;
        Total = total;

        ListaNotificacao = new List<GenericNotificationResponse>();

        if (listNotification != null)
        {
            foreach (var mensagem in listNotification)
                ListaNotificacao.Add(new GenericNotificationResponse(mensagem, notificatioNType));
        }
    }

    public void LoadPagination(
        IQueryable<T> query,
        GenericSearchPaginationRequest genericPaginationSearchResponse)
    {
        if (genericPaginationSearchResponse.IgnorarPaginacao)
            ListaResultado = query.ToList();
        else
        {
            ListaResultado = query.Skip(genericPaginationSearchResponse.Skip)
                              .Take(genericPaginationSearchResponse.TamanhoPagina)
                              .ToList();
        }

        Total = query.Count();
    }
}