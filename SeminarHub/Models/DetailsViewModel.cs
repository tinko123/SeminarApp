using static SeminarHub.Data.Models.DataConstants;

namespace SeminarHub.Models
{
    public class DetailsViewModel
    {
        public DetailsViewModel(
            int id,
            string topic,
            string lecturer,
            string category,
            DateTime dateAndTime,
            string organizer,
            string details,
            int duration)
        {
            Id = id;
            Topic = topic;
            Lecturer = lecturer;
            Category = category;
            DateAndTime = dateAndTime.ToString(DateTimeFormat);
            Organizer = organizer;
            Details = details;
            Duration = duration;
        }
        public int Id { get; set; }
        public int Duration { get; set; }
        public string Topic { get; set; }
        public string Lecturer { get; set; }
        public string Category { get; set; }
        public string DateAndTime { get; set; }
        public string Organizer { get; set; }
        public string Details { get; set; }

    }
}
