﻿using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace DataService.Models
{
    public class User
    {
        public long Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public string Surname { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; } = (DateTime)SqlDateTime.MinValue;
        
        public string Email { get; set; } = string.Empty;
        
        public string Identifier { get; set; } = string.Empty;

        public UserLevel Level { get; set; } = UserLevel.User;
        
        public string Nationality { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public List<TimeStamp> TimeStamps { get; set; }
    }
}
