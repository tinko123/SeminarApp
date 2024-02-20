using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Intrinsics.X86;
using System.Security.Policy;
using static SeminarHub.Data.Models.DataConstants;

namespace SeminarHub.Data.Models
{
    public class Seminar
    {
        [Key]
        [Comment("Primary Key")]
        public int Id { get; set; }
        [Required]
        [MaxLength(TopicMaxLength)]
        [Comment("The topic of the seminar")]
        public string Topic { get; set; } = string.Empty;
        [Required]
        [MaxLength(LecturerMaxLength)]
        [Comment("The lecturer of the seminar")]
        public string Lecturer { get; set; } = string.Empty;
        [Required]
        [MaxLength(DetailsMaxLength)]
        [Comment("The details of the seminar")]
        public string Details { get; set; } = string.Empty;
        [Required]
        [Comment("The organizer of the seminar")]
        public string OrganizerId { get; set; } = string.Empty;
        [Required]
        [Comment("The organizer of the seminar")]
        public IdentityUser Organizer { get; set; } = null!;
        [Required]
        [Comment("The date and time of the seminar")]
        public DateTime DateAndTime { get; set; }
        public int Duration { get; set; }
        [Required]
        [Comment("The category of the seminar")]
        public int CategoryId { get; set; }
        [Required]
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;
        public IList<SeminarParticipant> SeminarsParticipants { get; set; } = new List<SeminarParticipant>();
    }
}
