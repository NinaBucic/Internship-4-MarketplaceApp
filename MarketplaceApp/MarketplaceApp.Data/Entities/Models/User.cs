﻿namespace MarketplaceApp.Data.Entities.Models
{
    public abstract class User
    {
        public string Name { get; set; }
        public string Email { get; set; }

        protected User(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
