using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Driver.PutDriverSetCnhImage.Interfaces;

public interface IPutDriverSetCnhImageUseCase
{
    Task Execute(
        long id,
        string base64String);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Driver> outputPort);
}