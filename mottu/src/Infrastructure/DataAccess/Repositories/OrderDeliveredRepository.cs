using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.DataAccess.Repositories;

public class OrderDeliveredRepository : Repository<OrderDelivered>, IOrderDeliveredRepository
{
    public OrderDeliveredRepository(
        MottuDbContext context) : base(context)
    {
    }
}