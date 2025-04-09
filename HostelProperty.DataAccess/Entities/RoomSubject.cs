namespace HostelProperty.DataAccess.Entities
{
    public class RoomSubject
    {
        public Guid Id { get; set; }

        public required string Title { get; set; }

        public required DateOnly DateReseption { get; set; }

        public int? RoomNumber { get; set; }

        public Room? Room { get; set; }
    }
}
