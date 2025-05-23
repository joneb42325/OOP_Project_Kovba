﻿using System.ComponentModel.DataAnnotations;

namespace OOP_Project_Kovba.ViewModels
{
    public class CreateTripViewModel 
    {

        [Required(ErrorMessage = "Вкажіть місто відправлення.")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Місто відправлення має бути від 2 до 15 символів.")]
        public string FromCity { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть вулицю і номер будинку.")]
        [RegularExpression(@"^[^,]+,\s*\d+[A-Za-zА-Яа-яІіЇїЄєҐґ]*$",
        ErrorMessage = "Введіть у форматі: Назва вулиці, номер будинку (наприклад, Шевченка, 12А).")]
        public string FromStreetAndHouse { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть місто прибуття.")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Місто прибуття має бути від 2 до 15 символів.")]
        public string ToCity { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть вулицю і номер будинку прибуття.")]
        public string ToStreetAndHouse { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть дату та час відправлення.")]
        [DataType(DataType.DateTime)]
        public DateTime DepartureDate { get; set; }

        [Required(ErrorMessage = "Вкажіть дату та час прибуття.")]
        [DataType(DataType.DateTime)]
        public DateTime ArrivalDate { get; set; }

        [Range(1, 4, ErrorMessage = "Кількість пасажирів має бути від 1 до 4.")]
        public int MaxPassengers { get; set; }

        [Required(ErrorMessage = "Вкажіть модель авто.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Модель авто має бути від 2 до 30 символів.")]
        public string CarModel { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть ціну.")]
        [Range(0, double.MaxValue, ErrorMessage = "Ціна повинна бути додатнім числом.")]
        public decimal Price { get; set; }

        public string? Comment { get; set; }


    }
}
