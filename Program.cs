using epp_be.Data;
using epp_be.Interfaces;
using epp_be.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DatabaseContext>(o => o.UseSqlServer(configuration.GetConnectionString("LocalConnection")));
builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<IAppointment, AppointmentService>();
builder.Services.AddScoped < IPayments,PaymentService>();
builder.Services.AddScoped<IFunctionalSkillsMessages,FunctionalSkillsMessagesService>();
builder.Services.AddScoped<IContactMessages,ContactMessagesService>();
builder.Services.AddScoped<ICourse, CourseService>();
builder.Services.AddScoped<ILesson, LessonService>();
builder.Services.AddScoped<IUserCourse, UserCourseService>();
builder.Services.AddScoped<IChatMessages, ChatMessagesService>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Auth:Secret"])),
            ValidateIssuerSigningKey = true,
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        options.RequireHttpsMetadata = false;
    });



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.UseCors(o => o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.MapControllers();

app.Run();
