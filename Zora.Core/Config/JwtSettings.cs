﻿namespace Zora.Core.Config;

public class JwtSettings
{
    public string Key { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public int ExpiresInDays { get; set; } = 7;
}
