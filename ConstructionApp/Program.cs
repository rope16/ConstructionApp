using ConstructionApp;
using ConstructionApp.Interfaces.ConstructionSite;
using ConstructionApp.Interfaces.ProjectInterfaces;
using ConstructionApp.Interfaces.ProjectTasksInterface;
using ConstructionApp.Interfaces.User;
using ConstructionApp.Interfaces.UserInterfaces;
using ConstructionApp.Services.ConstructionSiteService;
using ConstructionApp.Services.ProjectServices;
using ConstructionApp.Services.ProjectTasksService;
using ConstructionApp.Services.UserServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var Configuration = builder.Configuration;
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserPasswordHasherService, UserPasswordHasherService>();
builder.Services.AddScoped<IConstructionSiteService, ConstructionSiteService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectTaskInterface, ProjectTaskService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
