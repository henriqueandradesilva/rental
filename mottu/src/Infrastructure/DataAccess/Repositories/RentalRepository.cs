using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.DataAccess.Repositories;

public class RentalRepository : Repository<Rental>, IRentalRepository
{
    public RentalRepository(
        MottuDbContext context) : base(context)
    {
    }
}