using System.ComponentModel.DataAnnotations;
using static SeminarHub.Data.Models.DataConstants;

namespace SeminarHub.Models
{
    public class AllViewModel
    {
        public AllViewModel(
            int id,
            string topic,
            string lecturer,
            string category,
            DateTime dateAndTime,
            string organizer
            )
        {
            Id = id;
            Topic = topic;
            Lecturer = lecturer;
            Category = category;
            DateAndTime = dateAndTime.ToString(DateTimeFormat);
            Organizer = organizer;
        }
        public int Id { get; set; }
        public string Topic { get; set; } = string.Empty;
        public string Lecturer { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string DateAndTime { get; set; } = string.Empty;
        public string Organizer { get; set; } = string.Empty;
    }
}
