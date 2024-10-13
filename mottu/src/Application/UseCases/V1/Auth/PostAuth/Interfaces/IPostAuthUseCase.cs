using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.Auth.Request;
using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Auth.PostAuth.Interfaces;

public interface IPostAuthUseCase
{
    Task Execute(
        PostAuthRequest postAuthRequest);

    void SetOutputPort(
        IOutputPortWithForbid<JwtResponse> outputPort);
}