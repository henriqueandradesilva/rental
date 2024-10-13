using System;

namespace CrossCutting.Settings;

public class RabbitMqSettings
{
    public string HostName { get; set; }

    public string Port { get; set; }

    public string PortUrl { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public TimeSpan CheckInterval { get; set; }

    public RabbitMqSettings()
    {

    }

    public RabbitMqSettings(
        string hostName,
        string port,
        string portUrl,
        string userName,
        string password,
        TimeSpan checkInterval)
    {
        HostName = hostName;
        Port = port;
        PortUrl = portUrl;
        UserName = userName;
        Password = password;
        CheckInterval = checkInterval;
    }
}
