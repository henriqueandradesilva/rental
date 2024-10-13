using CrossCutting.Const;
using Domain.Common.Enums;
using Domain.Common.Extensions;
using System.Security.Claims;

namespace CrossCutting.Extensions.UseCases;

public static class ValidateUserExtension
{
    public static bool IsOwnerOrAdminByUserId(
        long id,
        ClaimsPrincipal user)
    {
        var userRole = user.FindFirst(ClaimTypes.Role)?.Value;

        if (userRole == SystemConst.Admin)
            return true;

        var userId = user.FindFirst("UserId")?.Value;

        if (userId == null)
            return false;

        if (id != long.Parse(userId))
            return false;

        return true;
    }

    public static bool IsOwnerOrAdminByDriverId(
        long id,
        ClaimsPrincipal user)
    {
        var userRole = user.FindFirst(ClaimTypes.Role)?.Value;

        if (userRole == SystemConst.Admin)
            return true;

        var driverId = user.FindFirst("DriverId")?.Value;

        if (driverId == null)
            return false;

        if (id != long.Parse(driverId))
            return false;

        return true;
    }

    public static bool CheckCnhType(
       string cnhType)
    {
        if (!ValidateExtension.IsCnhTypeValid(cnhType, out CnhTypeEnum cnhTypeEnum))
            return false;

        return true;
    }
}