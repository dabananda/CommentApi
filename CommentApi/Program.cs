using CommentApi.Common;
using CommentApi.Common.Abstraction;
using CommentApi.Data;
using CommentApi.Features.Comments.CreateComment;
using CommentApi.Repositories;
using CommentApi.Repositories.Implementations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var settings = new GlobalSettings();
builder.Configuration.Bind(settings);
builder.Services.AddSingleton(settings);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CommentDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Portfolio", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://dabananda.vercel.app")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Custom mediator services
builder.Services.AddScoped<ISender, Sender>();
builder.Services.AddRequestHandlers(typeof(Program).Assembly);

// FluentValidation services
builder.Services.AddValidatorsFromAssemblyContaining<CreateCommentCommandValidator>();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddScoped<ICommentRepository, CommentRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Portfolio");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
