namespace HostelProperty.DataAccess.Entities
{
    public class Resident
    {
        public Guid Id { get; set; }

        public required string FirstName { get; set; }

        public required string MiddleName { get; set; }

        public required string LastName { get; set; }

        public int Age { get; set; }

        public int NumberCourse { get; set; }

        public required int RoomId { get; set; }

        public List<Subject>? Subjects { get; set; }
    }
}
