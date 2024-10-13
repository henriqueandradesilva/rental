using CrossCutting.Enums;
using Serilog;
using System.Collections.Generic;

namespace CrossCutting.Common.Dtos.Response;

public class GenericResponse<T>
{
    public bool Sucesso { get; set; }

    public T Resultado { get; set; }

    public List<GenericNotificationResponse> ListaNotificacao { get; set; }

    public GenericResponse()
    {

    }

    public GenericResponse(
        bool success,
        T result,
        List<string> listNotification,
        NotificationTypeEnum notificationType)
    {
        Sucesso = success;
        Resultado = result;
        ListaNotificacao = new List<GenericNotificationResponse>();

        if (listNotification != null)
        {
            foreach (var message in listNotification)
            {
                switch (notificationType)
                {
                    case NotificationTypeEnum.Success:
                        Log.Debug(message);
                        break;
                    case NotificationTypeEnum.Warning:
                        Log.Debug(message);
                        break;
                    case NotificationTypeEnum.Error:
                        Log.Error(message);
                        break;
                }

                ListaNotificacao.Add(new GenericNotificationResponse(message, notificationType));
            }
        }
    }
}