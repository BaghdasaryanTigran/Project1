using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rm.BLL;
using Rm.BLL.Interfaces;
using Rm.DAL;
using Rm.DAL.Context;
using Rm.DAL.Repositories;

using Rm.DAL.Repositories.Interface;
using Rm.Model.Models;
using Rm.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//aaaaalklk
// Add services to the container.
var Config = builder.Configuration;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Config["Jwt:Issuer"],
        ValidAudience = Config["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Jwt:Key"]))
    };
}
);
builder.Services.AddControllers();
builder.Services.AddDbContext<RmContext>(x => x.UseSqlServer("Server=localhost;Database=Rem;Trusted_Connection=True;TrustServerCertificate=true;"));
builder.Services.AddScoped<IBaseRepository<Document>, BaseRepository<Document>>();
builder.Services.AddScoped<IBaseRepository<Car>, BaseRepository<Car>>();
builder.Services.AddScoped<IBaseRepository<Worker>, BaseRepository<Worker>>();
//builder.Services.AddScoped<IBaseRepository<User>, BaseRepository < User >> ();
builder.Services.AddScoped<IBaseRepository<Document>, BaseRepository<Document>>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService > ();
builder.Services.AddScoped<ITokenService, TokenService > ();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IWorkerService, WorkerService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestLineSize = 1024 * 1024; 
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                              new OpenApiSecurityScheme {
                                  Reference = new OpenApiReference {
                                        Type = ReferenceType.SecurityScheme,
                                             Id = "Bearer"
                                  }
                              },
                          new string[] {}
                   }
                }); ;

});







var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "JWTToken_Auth_API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
