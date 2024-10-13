using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.Order.Request;

public class PutOrderSetStatusRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.OrderStatusIdRequired)]
    public long StatusId { get; set; }

    public PutOrderSetStatusRequest()
    {

    }

    public PutOrderSetStatusRequest(
        long statusId)
    {
        StatusId = statusId;
    }
}