using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.DataAccess.Repositories;

public class PlanTypeRepository : Repository<PlanType>, IPlanTypeRepository
{
    public PlanTypeRepository(
        MottuDbContext context) : base(context)
    {
    }
}