using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderWorker.Worker.Interfaces;

public interface IDriverNotificatedWorker : IDisposable
{
    Task Init(
        CancellationToken cancellationToken);

    Task Start(
        CancellationToken cancellationToken);
}