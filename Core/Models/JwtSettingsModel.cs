﻿namespace Core.Models
{
    public class JwtSettingsModel
    {
        public required string Key { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
    }
}
