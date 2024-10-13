namespace Domain.Common.Consts;

public class MessageConst
{
    #region Default

    public const string DescriptionRequired = "O campo Descrição é obrigatório.";

    public const string DescriptionMaxPermitted = "O máximo permitido para o campo Descrição é de 100 caracteres.";

    public const string MessageEmpty = "Nenhuma mensagem disponível.";

    public const string ActionNotPermitted = "Ação não permitida.";

    public const string OrderStatusActionPermitted = "O pedido já foi aceito/entregue/cancelado.";

    #endregion

    #region Driver

    public const string DriverCreated = "Entregador cadastrado com sucesso.";

    public const string DriverUpdated = "Entregador modificado com sucesso.";

    public const string DriverNotExist = "Nenhum Entregador foi encontrado.";

    public const string DriverExist = "Este Entregador já existe.";

    public const string DriverIsDelivering = "Entregador está realizando uma entrega.";

    public const string DriverIdentifierRequired = "O campo Identificador é obrigatório.";

    public const string DriverNameRequired = "O campo Nome é obrigatório.";

    public const string DriverNameMaxPermitted = "O máximo permitido para o campo Nome é de 150 caracteres.";

    public const string DriverCnpjRequired = "O campo Cnpj é obrigatório.";

    public const string DriverCnpjInvalid = "O campo Cnpj é inválido.";

    public const string DriverCnhRequired = "O campo Cnh é obrigatório.";

    public const string DriverCnhInvalid = "O campo Cnh é inválido.";

    public const string DriverDateOfBirthRequired = "O campo Data Nascimento é obrigatório.";

    public const string DriverDateOfBirthInvalid = "A Data de nascimento informada é inválida.";

    public const string DriverDateOfBirthRuleInvalid = "A Data de Nascimento deve ser anterior a";

    public const string DriverCnhTypeRequired = "O campo Tipo da Cnh é obrigatório.";

    public const string DriverCnhTypeInvalid = "O campo Tipo da Cnh é inválido, os tipos válidos são (A, B, or A+B).";

    public const string DriverCnhImageRequired = "O campo Foto da Cnh é obrigatório.";

    public const string DriverDeliveringRequired = "O campo Entregando é obrigatório.";

    public const string DriverImageTypeInvalid = "Imagem inválida! Por favor, envie apenas imagens no formato PNG ou BMP.";

    #endregion

    #region Driver Notificated

    public const string DriverNotificated = "Entregador notificado com sucesso.";

    public const string DriverNotificatedNotExist = "Nenhum Entregador Notificado foi encontrado.";

    public const string DriverNotificatedExist = "Este Entregador Já foi Notificado.";

    public const string DriverNotificatedDriverIdRequired = "O campo Entregador é obrigatório.";

    public const string DriverNotificatedNotificationIdRequired = "O campo Notificação é obrigatório.";

    public const string DriverNotificatedDateRequired = "O campo Data é obrigatório.";

    public const string DriverNotificatedDateInvalid = "A Data da Notificação informada é inválida.";

    #endregion

    #region Model Vehicle

    public const string ModelVehicleCreated = "Modelo de Veículo cadastrado com sucesso.";

    public const string ModelVehicleUpdated = "Modelo de Veículo modificado com sucesso.";

    public const string ModelVehicleNotExist = "Nenhum Modelo de Veículo foi encontrado.";

    public const string ModelVehicleExist = "Este Modelo de Veículo já existe.";

    #endregion

    #region Motorcycle

    public const string MotorcycleCreated = "Moto cadastrada com sucesso.";

    public const string MotorcycleUpdated = "Moto modificada com sucesso.";

    public const string MotorcyclePlateUpdated = "Placa modificada com sucesso.";

    public const string MotorcycleNotExist = "Nenhuma Moto foi encontrada.";

    public const string MotorcycleIsRented = "Moto já foi alugada.";

    public const string MotorcycleExist = "Esta Moto já existe.";

    public const string MotorcycleIdentifierRequired = "O campo Identificador é obrigatório.";

    public const string MotorcycleIdentifierMaxPermitted = "O máximo permitido para o campo Identificador é de 100 caracteres.";

    public const string MotorcycleYearRequired = "O campo Ano é obrigatório.";

    public const string MotorcyclePlateRequired = "O campo Placa é obrigatório.";

    public const string MotorcycleModelVehicleIdRequired = "O campo Modelo de Véiculo é obrigatório.";

    public const string MotorcycleRentedRequired = "O campo Alugado é obrigatório.";

    public const string MotorcyclePlateMaxPermitted = "O máximo permitido para o campo Placa é de 8 caracteres.";

    #endregion

    #region Motorcycle Event Created

    public const string MotorcycleEventCreatedMotorcycleIdRequired = "O campo Moto é obrigatório.";

    public const string MotorcycleEventCreatedJsonRequired = "O campo Json é obrigatório.";

    #endregion

    #region Notification

    public const string NotificationCreated = "Notificação cadastrada com sucesso.";

    public const string NotificationUpdated = "Notificação modificada com sucesso.";

    public const string NotificationNotExist = "Nenhuma Notificação foi encontrada.";

    public const string NotificationExist = "Esta Notificação já existe.";

    public const string NotificationOrderIdRequired = "O campo Pedido é obrigatório.";

    public const string NotificationDateRequired = "O campo Data da Notificação é obrigatório.";

    public const string NotificationDateInvalid = "O campo Data da Notificação é inválida.";

    #endregion

    #region Order

    public const string OrderCreated = "Pedido cadastrado com sucesso.";

    public const string OrderUpdated = "Pedido modificado com sucesso.";

    public const string OrderNotExist = "Nenhum Pedido foi encontrado.";

    public const string OrderExist = "Este Pedido já existe";

    public const string OrderStatusIdRequired = "O campo Situação é obrigatório.";

    public const string OrderValueRequired = "O campo Valor é obrigatório.";

    public const string OrderDateRequired = "O campo Data é obrigatório.";

    public const string OrderDateInvalid = "A Data do Pedido informada é inválida.";

    #endregion

    #region Order Accepted

    public const string OrderAccepted = "Pedido aceito com sucesso.";

    public const string OrderAcceptedNotExist = "Nenhum Pedido foi aceito.";

    public const string OrderAcceptedExist = "Este Pedido já foi aceito.";

    public const string OrderAcceptedDriverIdRequired = "O campo Entregador é obrigatório.";

    public const string OrderAcceptedOrderIdRequired = "O campo Pedido é obrigatório.";

    public const string OrderAcceptedDateRequired = "O campo Data do Aceite é obrigatório.";

    public const string OrderAcceptedDateInvalid = "A Data do Aceite informada é inválida.";

    #endregion

    #region Order Delivered

    public const string OrderDelivered = "Pedido entregue com sucesso.";

    public const string OrderDeliveredNotExist = "Nenhum Pedido foi entregue.";

    public const string OrderDeliveredExist = "Este Pedido já foi entregue.";

    public const string OrderDeliveredDriverIdRequired = "O campo Entregador é obrigatório.";

    public const string OrderDeliveredOrderIdRequired = "O campo Pedido é obrigatório.";

    public const string OrderDeliveredDateRequired = "O campo Data da Entrega é obrigatória.";

    public const string OrderDeliveredDateInvalid = "A Data da Entrega informada é inválida.";

    #endregion

    #region Order Status

    public const string OrderStatusCreated = "Status do Pedido cadastrado com sucesso.";

    public const string OrderStatusUpdated = "Status do Pedido modificado com sucesso.";

    public const string OrderStatusNotExist = "Nenhum Status do Pedido foi encontrado.";

    public const string OrderStatusExist = "Este Status do Pedido já existe.";

    #endregion

    #region Plan

    public const string PlanCreated = "Plano cadastrado com sucesso.";

    public const string PlanUpdated = "Plano modificado com sucesso.";

    public const string PlanNotExist = "Nenhum Plano foi encontrado.";

    public const string PlanExist = "Este Plano já existe.";

    public const string PlanIsNotActive = "Este Plano não está ativo.";

    public const string PlanTypeIdRequired = "O campo Tipo do Plano é obrigatório.";

    public const string PlanDailyRateRequired = "O campo Taxa Diária é obrigatória.";

    public const string PlanAdditionalRateRequired = "O campo Taxa Extra é obrigatória.";

    public const string PlanDailyLateFeeRequired = "O campo Taxa Fixa é obrigatória.";

    public const string PlanDurationInDaysRequired = "O campo Duração em Dias é obrigatório.";

    public const string PlanActiveRequired = "O campo Ativo é obrigatório.";

    #endregion

    #region Plan Type

    public const string PlanTypeCreated = "Tipo do Plano cadastrado com sucesso.";

    public const string PlanTypeUpdated = "Tipo do Plano modificado com sucesso.";

    public const string PlanTypeNotExist = "Nenhum Tipo de Plano foi encontrado.";

    public const string PlanTypeExist = "Este Tipo de Plano já existe.";

    #endregion

    #region Rental

    public const string RentalCreated = "Locação cadastrada com sucesso.";

    public const string RentalUpdated = "Locação modificada com sucesso.";

    public const string RentalNotExist = "Nenhuma Locação foi encontrada.";

    public const string RentalIsReturn = "Locação já finalizada.";

    public const string RentalMotorcycleIdRequired = "O campo Moto é obrigatório.";

    public const string RentalDriverIdRequired = "O campo Entregador é obrigatório.";

    public const string RentalPlanIdRequired = "O campo Plano é obrigatório.";

    public const string RentalStartDateRequired = "O campo Data de Início é obrigatório.";

    public const string RentalStartDateInvalid = "A Data de Início é inválida.";

    public const string RentalStartDateRuleInvalid = "A data de início deve ser posterior a hoje.";

    public const string RentalEndDateRequired = "O campo Data de Devolução é obrigatório.";

    public const string RentalEndDateInvalid = "O campo Data de Devolução é inválida.";

    public const string RentalEndDateMustBeAfterStartDate = "A Data de Término deve ser posterior à Data de Início.";

    public const string RentalDriverNotCnhTypeA = "Somente entregadores habilitados na categoria A podem efetuar uma locação.";

    #endregion

    #region User

    public const string UserCreated = "Usuário cadastrado com sucesso.";

    public const string UserUpdated = "Usuário modificado com sucesso.";

    public const string UserNotExist = "Nenhum Usuário foi encontrado.";

    public const string UserExist = "Este Usuário já existe.";

    public const string UserIsNotActive = "Este Entregador não está ativo.";

    public const string UserUserRoleIdRequired = "O campo Perfil é obrigatório.";

    public const string UserNameRequired = "O campo Nome é obrigatório.";

    public const string UserNameMaxPermitted = "O máximo permitido para o campo Nome é de 100 caracteres.";

    public const string UserEmailRequired = "O campo E-mail é obrigatório.";

    public const string UserEmailMaxPermitted = "O máximo permitido para o campo E-mail é de 255 caracteres.";

    public const string UserEmailInvalid = "O formato do e-mail é inválido.";

    public const string UserPasswordRequired = "O campo Senha é obrigatório.";

    public const string UserPasswordMaxPermitted = "O máximo permitido para o campo Senha é de 255 caracteres.";

    public const string UserActiveRequired = "O campo Ativo é obrigatório.";

    #endregion

    #region User Role

    public const string UserRoleCreated = "Tipo de Perfil cadastrado com sucesso.";

    public const string UserRoleUpdated = "Tipo de Perfil modificado com sucesso.";

    public const string UserRoleNotExist = "Nenhum Tipo de Perfil foi encontrado.";

    public const string UserRoleExist = "Este Tipo de Perfil já existe";

    #endregion
}