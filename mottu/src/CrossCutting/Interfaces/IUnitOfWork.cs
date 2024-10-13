using System.Threading.Tasks;

namespace CrossCutting.Interfaces;

public interface IUnitOfWork
{
    Task<string> Save();
}