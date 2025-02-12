using ConstructionApp;
using ConstructionApp.Interfaces.AuthInterfaces;
using ConstructionApp.Interfaces.ConstructionSite;
using ConstructionApp.Interfaces.ProjectInterfaces;
using ConstructionApp.Interfaces.ProjectTasksInterface;
using ConstructionApp.Interfaces.User;
using ConstructionApp.Interfaces.UserInterfaces;
using ConstructionApp.Interfaces.UserTaskInterfaces;
using ConstructionApp.Services.AuthServices;
using ConstructionApp.Services.ConstructionSiteService;
using ConstructionApp.Services.ProjectServices;
using ConstructionApp.Services.ProjectTasksService;
using ConstructionApp.Services.UserServices;
using ConstructionApp.Services.UserTaskService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var Configuration = builder.Configuration;
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserPasswordHasherService, UserPasswordHasherService>();
builder.Services.AddScoped<IConstructionSiteService, ConstructionSiteService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectTaskInterface, ProjectTaskService>();
builder.Services.AddScoped<UserTaskServices, UserTaskService>();
builder.Services.AddScoped<ITokenProviderServices, TokenProviderService>();
builder.Services.AddScoped<IUserAuthServices, UserAuthService>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
