using CrossCutting.Const;
using Serilog;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Helpers;

public class NotificationHelper
{
    public readonly IDictionary<string, IList<string>> Messages =
        new Dictionary<string, IList<string>>();

    public bool HasMessage
        => Messages.Any();

    public void Add(
        string key,
        string message)
    {
        if (!Messages.ContainsKey(key))
            Messages[key] = new List<string>();

        Messages[key].Add(message);

        if (key == SystemConst.Error)
            Log.Error(message);
        else if (key == SystemConst.NotFound)
            Log.Debug(message);
    }
}