namespace CrossCutting.Const;

public class SystemConst
{
    #region Default

    public const string MotorcycleEventCreatedQueue = "motorcycle_event_created_queue";

    public const string OrderEventCreatedQueue = "order_event_created_queue";

    public const long UserAdminIdDefault = 1;

    public const long UserRoleAdminIdDefault = 1;

    public const long UserRoleDriverIdDefault = 2;

    public const long OrderStatusAvailableDefault = 1;

    public const long OrderStatusAcceptedDefault = 2;

    public const long OrderStatusDeliveredDefault = 3;

    public const long OrderStatusCancelledDefault = 4;

    public const long PlanSevenDays = 7;

    public const long PlanFifteenDays = 15;

    public const long PlanThirtyDays = 30;

    public const long PlanFortyFiveDays = 45;

    public const long PlanFiftyDays = 50;

    public const long PlanTypeStartDefault = 1;

    public const long PlanTypeProDefault = 2;

    public const long PlanTypePrimeDefault = 3;

    public const string Admin = "ADMIN";

    public const string Driver = "ENTREGADOR";

    public const string Error = "Error";

    public const string NotFound = "NotFound";

    public const string Forbid = "Forbid";

    public const string FieldUserId = "userId";

    public const string FieldUserRoleId = "userRoleId";

    public const string FieldMotorcycleId = "motorcycleId";

    public const string FieldDriverId = "driverId";

    public const string FieldPlanId = "planId";

    public const string FieldOrderId = "orderId";

    public const string FieldStatusId = "statusId";

    public const string FieldNotificationId = "notificationId";

    public const string FieldCnhType = "cnhType";

    public const string FieldModelVehicleId = "modelVehicleId";

    public const string FieldRentalStatus = "rentalStatus";

    #endregion

    #region Auth

    public const string AuthEmailRequired = "E-mail não informado.";

    public const string AuthEmailInvalid = "O formato do e-mail é inválido.";

    public const string AuthPasswordRequired = "Senha não informada.";

    public const string AuthEmailOrPasswordInvalid = "E-mail ou Senha inválida.";

    public const string AuthAccountIsNotActive = "Sua conta está inativa, por favor entre em contato com o administrador.";

    #endregion
}