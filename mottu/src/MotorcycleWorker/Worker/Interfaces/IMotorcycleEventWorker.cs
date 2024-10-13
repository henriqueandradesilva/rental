using System;
using System.Threading;
using System.Threading.Tasks;

namespace MotorcycleWorker.Worker.Interfaces;

public interface IMotorcycleEventWorker : IDisposable
{
    Task Init(
        CancellationToken cancellationToken);

    Task Start(
        CancellationToken cancellationToken);
}