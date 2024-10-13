using CrossCutting.Const;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.DataAccess.Seeds;

public static class SeedData
{
    public static void Seed(ModelBuilder builder)
    {
        if (builder == null)
            throw new ArgumentNullException(nameof(builder));

        builder.Entity<ModelVehicle>()
               .HasData(
                    new ModelVehicle(1, "MOTTU SPORT"));

        builder.Entity<OrderStatus>()
               .HasData(
                    new OrderStatus(SystemConst.OrderStatusAvailableDefault, "DISPONÍVEL"),
                    new OrderStatus(SystemConst.OrderStatusAcceptedDefault, "ACEITO"),
                    new OrderStatus(SystemConst.OrderStatusDeliveredDefault, "ENTREGUE"),
                    new OrderStatus(SystemConst.OrderStatusCancelledDefault, "CANCELADO"));

        builder.Entity<PlanType>()
               .HasData(
                    new PlanType(SystemConst.PlanTypeStartDefault, "START"),
                    new PlanType(SystemConst.PlanTypeProDefault, "PRO"),
                    new PlanType(SystemConst.PlanTypePrimeDefault, "PRIME"));

        builder.Entity<Plan>()
               .HasData(
                    new Plan(SystemConst.PlanSevenDays, 1, "7 DIAS", 30.00, 0.2, 50, 7, true),
                    new Plan(SystemConst.PlanFifteenDays, 2, "15 DIAS", 28.00, 0.4, 50, 15, true),
                    new Plan(SystemConst.PlanThirtyDays, 2, "30 DIAS", 22.00, 0.6, 50, 30, true),
                    new Plan(SystemConst.PlanFortyFiveDays, 3, "45 DIAS", 20.00, 0.8, 50, 45, true),
                    new Plan(SystemConst.PlanFiftyDays, 3, "50 DIAS", 18.00, 1.0, 50, 50, true));

        builder.Entity<UserRole>()
               .HasData(
                    new UserRole(SystemConst.UserRoleAdminIdDefault, SystemConst.Admin),
                    new UserRole(SystemConst.UserRoleDriverIdDefault, SystemConst.Driver));

        var userAdmin = new User(SystemConst.UserAdminIdDefault, 1, "ADMIN", "ADMIN@MOTTU.APP", "1", true);

        builder.Entity<User>()
               .HasData(userAdmin);
    }
}
