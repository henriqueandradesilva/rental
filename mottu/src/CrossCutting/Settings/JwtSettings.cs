namespace CrossCutting.Settings;

public class JwtSettings
{
    public string Secret { get; set; }

    public string Audience { get; set; }

    public string Issuer { get; set; }

    public int TotalHoursExpiresToken { get; set; }

    public JwtSettings()
    {

    }

    public JwtSettings(
        string secret,
        string audience,
        string issuer,
        int totalHoursExpiresToken)
    {
        Secret = secret;
        Audience = audience;
        Issuer = issuer;
        TotalHoursExpiresToken = totalHoursExpiresToken;
    }
}