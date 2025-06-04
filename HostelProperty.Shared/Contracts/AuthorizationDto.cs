namespace HostelProperty.Shared.Contracts
{
    public record class AuthorizationDto(
        string Email,
        string Password);
}
