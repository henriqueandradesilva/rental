using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.DataAccess.Repositories;

public class DriverNotificatedRepository : Repository<DriverNotificated>, IDriverNotificatedRepository
{
    public DriverNotificatedRepository(
        MottuDbContext context) : base(context)
    {
    }
}