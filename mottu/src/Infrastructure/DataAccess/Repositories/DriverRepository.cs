using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.DataAccess.Repositories;

public class DriverRepository : Repository<Driver>, IDriverRepository
{
    public DriverRepository(
        MottuDbContext context) : base(context)
    {
    }
}