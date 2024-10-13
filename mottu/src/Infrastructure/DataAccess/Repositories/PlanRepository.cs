using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.DataAccess.Repositories;

public class PlanRepository : Repository<Plan>, IPlanRepository
{
    public PlanRepository(
        MottuDbContext context) : base(context)
    {
    }
}