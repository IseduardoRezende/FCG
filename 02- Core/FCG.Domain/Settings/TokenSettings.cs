namespace FCG.Domain.Settings;

public class TokenSettings
{
    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public string Key { get; set; } = string.Empty;

    public int DaysUntilExpires { get; set; }
}
