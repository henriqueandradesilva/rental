using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.DataAccess.Repositories;

public class OrderAcceptedRepository : Repository<OrderAccepted>, IOrderAcceptedRepository
{
    public OrderAcceptedRepository(
        MottuDbContext context) : base(context)
    {
    }
}