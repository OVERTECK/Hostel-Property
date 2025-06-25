namespace HostelProperty.Shared.Contracts;

public record class FloorDto(
    int Id,
    string Title)
{
    public override string ToString()
    {
        return this.Title;
    }
}
