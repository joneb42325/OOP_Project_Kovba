using System.ComponentModel.DataAnnotations;

namespace OOP_Project_Kovba.ViewModels
{
    public class CreateTripViewModel : IValidatableObject
    {

        [Required(ErrorMessage = "Вкажіть місто відправлення.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Місто відправлення має бути від 3 до 20 символів.")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "Назва міста не повинна містити цифри")]
        public string FromCity { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть вулицю і номер будинку.")]
        [RegularExpression(@"^[^,]+,\s*\d+[A-Za-zА-Яа-яІіЇїЄєҐґ]*$",
        ErrorMessage = "Введіть у форматі: Назва вулиці, номер будинку (наприклад, Шевченка, 12А).")]
        public string FromStreetAndHouse { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть місто прибуття.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Місто прибуття має бути від 3 до 20 символів.")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "Назва міста не повинна містити цифри")]
        public string ToCity { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть вулицю і номер будинку прибуття.")]
        [RegularExpression(@"^[^,]+,\s*\d+[A-Za-zА-Яа-яІіЇїЄєҐґ]*$",
        ErrorMessage = "Введіть у форматі: Назва вулиці, номер будинку (наприклад, Шевченка, 12А).")]
        public string ToStreetAndHouse { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть дату та час відправлення.")]
        [DataType(DataType.DateTime)]
        [CustomDateValidation(ErrorMessage = "Дата повинна бути не раніше сьогоднішньої.")]
        public DateTime DepartureDate { get; set; }

        [Required(ErrorMessage = "Вкажіть дату та час прибуття.")]
        [DataType(DataType.DateTime)]
        [CustomDateValidation(ErrorMessage = "Дата повинна бути не раніше сьогоднішньої.")]
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DepartureDate >= ArrivalDate)
            {
                yield return new ValidationResult(
                    "Дата відправлення повинна бути раніше дати прибуття",
                    new[] { nameof(DepartureDate), nameof(ArrivalDate) });
            }
        }
    }
}
