using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HostelProperty.DataAccess.Entities
{
    public class RoomSubject
    {
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string? Title { get; set; }

        public DateTime? DateReseption { get; set; }

        public Guid? RoomId { get; set; }

        [JsonIgnore]
        public Room? Room { get; set; }
    }
}
