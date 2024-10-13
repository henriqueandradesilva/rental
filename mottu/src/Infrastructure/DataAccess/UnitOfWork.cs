using CrossCutting.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess;

public sealed class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly MottuDbContext _context;

    private bool _disposed;

    public UnitOfWork(
        MottuDbContext context) => _context = context;

    public void Dispose() => Dispose(true);

    public async Task<string> Save()
    {
        try
        {
            long affectedRows =
                await _context.SaveChangesAsync()
                              .ConfigureAwait(false);

            return string.Empty;
        }
        catch (DbUpdateException ex)
        {
            var message = string.Empty;

            var sqlException = ex.InnerException as NpgsqlException;

            if (sqlException != null &&
                sqlException.SqlState == "23503")
            {
                var deletedEntries =
                    _context.ChangeTracker?.Entries()
                                          ?.Where(e => e.State == EntityState.Deleted)
                                          ?.ToList();

                if (deletedEntries.Any())
                {
                    message = "Erro ao tentar excluir: existem registros dependentes que precisam ser removidos primeiro.";

                    return message;
                }

                message = "Erro ao tentar inserir/atualizar: chave estrangeira não encontrada.";

                return message;
            }

            var detailedMessage = ex.InnerException?.Message ?? ex.Message;

            message = $"Erro inesperado: {detailedMessage}\n{ex.StackTrace}";

            Log.Error(message);

            return message;
        }
        catch (Exception ex)
        {
            var detailedMessage = ex.InnerException?.Message ?? ex.Message;

            var message = $"Erro inesperado: {detailedMessage}\n{ex.StackTrace}";

            Log.Error(message);

            return message;
        }
    }

    private void Dispose(
        bool disposing)
    {
        if (!_disposed && disposing)
            _context.Dispose();

        _disposed = true;
    }
}