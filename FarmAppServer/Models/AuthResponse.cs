using System;
using FarmApp.Domain.Core.Entity;
using Newtonsoft.Json;

namespace FarmAppServer.Models
{
    public class AuthResponse
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime ResponseTime { get; set; } = DateTime.Now;
        public string Header { get; set; } = "Ok!";
        public string Token { get; set; }
        public User User { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}