using Humanizer;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;
using System.Security.Policy;
using static SeminarHub.Data.Models.DataConstants;

namespace SeminarHub.Models
{
    public class AddViewModel
    {
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(
            TopicMaxLength,
            MinimumLength = TopicMinLength,
            ErrorMessage = StringLengthErrorMessage
            )]
        public string Topic { get; init; } = string.Empty;
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(
            LecturerMaxLength,
            MinimumLength = LecturerMinLength,
            ErrorMessage = StringLengthErrorMessage
            )]
        public string Lecturer { get; init; } = string.Empty;
        [Required(ErrorMessage = RequiredErrorMessage)]
        public int CategoryId { get; init; }
        [Required(ErrorMessage = RequiredErrorMessage)]
        public string DateAndTime { get; init; } = string.Empty;
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(
            DetailsMaxLength,
            MinimumLength = DetailsMinLength,
            ErrorMessage = StringLengthErrorMessage
            )]
        public string Details { get; init; } = string.Empty;
        [Range(DurationMin, DurationMax,ErrorMessage = RangeErrorMessage)]
        public int Duration { get; init; } 
        public IEnumerable<CategoriesViewModel> Categories { get; set; } = new List<CategoriesViewModel>();

    }
}
