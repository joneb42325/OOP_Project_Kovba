using System.ComponentModel.DataAnnotations;

namespace OOP_Project_Kovba.ViewModels
{
    public class SearchTripViewModel
    {
        [Required(ErrorMessage = "Місто відправлення є обов'язковим")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Довжина міста відправлення повинна бути від 3 до 20 символів")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "Назва міста не повинна містити цифри")]
        public string From { get; set; } = string.Empty;

        [Required(ErrorMessage = "Місто прибуття є обов'язковим")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Довжина міста прибуття повинна бути від 3 до 20 символів")]
        public string To { get; set; } = string.Empty;

        [Required(ErrorMessage = "Дата поїздки є обов'язковою.")]
        [DataType(DataType.Date)]
        [CustomDateValidation(ErrorMessage = "Дата повинна бути не раніше сьогоднішньої.")]
        public DateTime Date { get; set; }

        [Range(1, 4, ErrorMessage = "Кількість пасажирів має бути від 1 до 4.")]
        public int Passengers { get; set; }
    }

    public class CustomDateValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime dateValue)
            {
                return dateValue.Date >= DateTime.Today;
            }
            return false;
        }
    }
}
