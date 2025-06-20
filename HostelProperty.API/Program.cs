using System.Text;
using HostelProperty.API.Endpoints;
using HostelProperty.DataAccess;
using HostelProperty.DataAccess.Configurations;
using HostelProperty.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ResidentRepository>();
builder.Services.AddScoped<RoomRepository>();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuerSigningKey = true,
        };
    });

var connectionString = builder.Configuration.GetConnectionString("ConnectionString");

builder.Services.AddDbContext<MyDbContext>(
    options =>
    {
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //using (var scope = app.Services.CreateScope())
    //{
    //    var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    //    DataSeeder.Seed(dbContext);
    //}

    app.MapOpenApi();
}
app.UseSwagger();
app.UseSwaggerUI();

//app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapAuthorizationEndpoint(builder);

app.MapRoomEndponts();

app.MapResidentEndpoints();

app.Run();