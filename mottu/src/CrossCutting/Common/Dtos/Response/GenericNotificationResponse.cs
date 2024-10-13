using CrossCutting.Enums;

namespace CrossCutting.Common.Dtos.Response;

public class GenericNotificationResponse
{
    public string Mensagem { get; set; }

    public NotificationTypeEnum TipoNotificacao { get; set; }

    public GenericNotificationResponse()
    {

    }

    public GenericNotificationResponse(
        string mensagem,
        NotificationTypeEnum tipoNotificacao)
    {
        Mensagem = mensagem;
        TipoNotificacao = tipoNotificacao;
    }
}