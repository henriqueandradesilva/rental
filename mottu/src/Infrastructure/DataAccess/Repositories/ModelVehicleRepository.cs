using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.DataAccess.Repositories;

public class ModelVehicleRepository : Repository<ModelVehicle>, IModelVehicleRepository
{
    public ModelVehicleRepository(
        MottuDbContext context) : base(context)
    {
    }
}