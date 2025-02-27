﻿namespace Core.Models
{
    public class HashingModel
    {
        public string Username { get; init; }
        public string Password { get; init; }
        public Guid Salt { get; init; }

        public HashingModel(string username, string password, Guid salt)
        {
            Username = username;
            Password = password;
            Salt = salt;
        }
    }
}
