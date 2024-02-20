using Humanizer;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using static SeminarHub.Data.Models.DataConstants;

namespace SeminarHub.Data.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(CategoryNameMaxLength)]
        public string Name { get; set; } = string.Empty;
        public IEnumerable<Seminar> Seminars { get; set; } = new List<Seminar>();

    }
}