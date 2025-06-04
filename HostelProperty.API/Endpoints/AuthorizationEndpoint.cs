using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HostelProperty.DataAccess;
using HostelProperty.Shared.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HostelProperty.API.Endpoints;

public static class AuthorizationEndpoint
{
    public static void MapAuthorizationEndpoint(this IEndpointRouteBuilder routes, WebApplicationBuilder builder)
    {
        var group = routes.MapGroup("authorization").WithTags("Authorization");

        group.MapPost("/", async (AuthorizationDto authorizationDto, MyDbContext dbContext) =>
        {

        var searchedUser = dbContext.Users.FirstOrDefaultAsync(c => c.Email == authorizationDto.Email).Result;

        if (searchedUser == null)
            return Results.NotFound();

        if (searchedUser.Password != authorizationDto.Password)
            return Results.Unauthorized();

        var claims = new List<Claim> { new Claim(ClaimTypes.Email, authorizationDto.Email) };

        var jwt = new JwtSecurityToken(
            issuer: builder.Configuration["Jwt:Issuer"],
            audience: builder.Configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromHours(2)),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                SecurityAlgorithms.HmacSha256)
            );

            return Results.Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
        });
    }
}
