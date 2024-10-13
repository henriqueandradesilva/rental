using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.DataAccess.Repositories;

public class MotorcycleEventCreatedRepository : Repository<MotorcycleEventCreated>, IMotorcycleEventCreatedRepository
{
    public MotorcycleEventCreatedRepository(
        MottuDbContext context) : base(context)
    {
    }
}