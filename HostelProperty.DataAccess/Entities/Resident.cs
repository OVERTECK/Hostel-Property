using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HostelProperty.DataAccess.Entities
{
    public class Resident
    {
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? MiddleName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        public byte Age { get; set; }

        public byte NumberCourse { get; set; }

        public Guid? RoomId { get; set; }

        [JsonIgnore]
        public Room? Room { get; set; }

        [JsonIgnore]
        public List<Subject>? Subjects { get; set; } = [];
    }
}
