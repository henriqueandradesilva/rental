namespace CrossCutting.Common.Dtos.Response;

public class JwtResponse
{
    public string Created { get; set; }

    public string Expiration { get; set; }

    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    public JwtResponse()
    {

    }

    public JwtResponse(
        string created,
        string expiration,
        string accessToken,
        string refreshToken,
        string message = null)
    {
        Created = created;
        Expiration = expiration;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}