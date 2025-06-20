using Bogus;
using HostelProperty.DataAccess.Entities;

namespace HostelProperty.DataAccess.Configurations;

public static class DataSeeder
{
    public static void Seed(MyDbContext dbContext)
    {
        // Generate rooms

        var numberRoom = 100;

        var fakeRooms = new Faker<Room>("ru")
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(c => c.Title, f => numberRoom++.ToString())
            .RuleFor(c => c.Floor, f => f.Random.Byte(1, 4))
            .RuleFor(c => c.CountResidents, f => f.Random.Byte(1, 4))
            .Generate(120);

        dbContext.AddRange(fakeRooms);
        dbContext.SaveChanges();

        // Generate residents
        var fakeResidents = new Faker<Resident>("ru")
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            .RuleFor(c => c.MiddleName, f => f.Name.FirstName())
            .RuleFor(c => c.LastName, f => f.Name.LastName())
            .RuleFor(c => c.Age, f => f.Random.Byte(14, 120))
            .RuleFor(c => c.NumberCourse, f => f.Random.Byte(1, 4))
            .RuleFor(c => c.RoomId, f => f.PickRandom(fakeRooms).Id)
            .Generate(360);

        dbContext.AddRange(fakeResidents);
        dbContext.SaveChanges();

        // Generate subjects
        var fakeSubjects = new Faker<Subject>("ru")
            .RuleFor(c => c.Id, faker => Guid.NewGuid())
            .RuleFor(c => c.Title, f => f.Commerce.ProductName())
            .RuleFor(c => c.DateReseption, f => f.Date.Past(2023))
            .RuleFor(c => c.ResidentId, f => f.PickRandom(fakeResidents).Id)
            .Generate(1500);

        dbContext.AddRange(fakeSubjects);
        dbContext.SaveChanges();

        // Generate room subjects
        var fakeRoomSubjects = new Faker<RoomSubject>("ru")
            .RuleFor(c => c.Id, faker => Guid.NewGuid())
            .RuleFor(c => c.Title, f => f.Commerce.ProductName())
            .RuleFor(c => c.DateReseption, f => f.Date.Past(2023))
            .RuleFor(c => c.RoomId, f => f.PickRandom(fakeRooms).Id)
            .Generate(1500);

        dbContext.AddRange(fakeRoomSubjects);
        dbContext.SaveChanges();
    }
}