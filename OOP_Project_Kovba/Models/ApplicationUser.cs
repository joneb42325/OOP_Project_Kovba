using Microsoft.AspNetCore.Identity;
using System;

namespace OOP_Project_Kovba.Models
{
    public class ApplicationUser : IdentityUser
    {
        private string _fullName = string.Empty;

        public string FullName
        {
            get => _fullName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Повне ім'я не може бути порожнім або містити лише пробіли");
                if (value.Length < 5)
                    throw new ArgumentException("Повне ім'я повинно містити принаймні 5 символів");
                _fullName = value;
            }
        }

        public void ChangeFullName(string newName)
        {
            FullName = newName;
        }
        
        public ICollection<Trip> trips { get; set; } = new List <Trip>();
        public ICollection<Booking> bookings { get; set; } = new List<Booking>();
    }
}
