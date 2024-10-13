using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services;

public class OutputPortService<T> : IOutputPort<T>
{
    private readonly NotificationHelper _notificationHelper;

    public T Result { get; set; }

    public List<string> Errors { get; set; } = new List<string>();

    public OutputPortService(
        NotificationHelper notificationHelper)
    {
        _notificationHelper = notificationHelper;
    }

    public void Ok(
        T result)
    {
        Result = result;
    }

    public void Error()
    {
        Errors = _notificationHelper.Messages[SystemConst.Error]?.ToList() ?? new List<string>();
    }
}