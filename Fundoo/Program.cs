
using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System.Text;

namespace Fundoo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            
           
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Fundoo API",
                    Version = "v1"
                });

                // Add Bearer token authentication
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {  
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Please enter a valid token"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                      Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
            });


            builder.Services.AddDbContext<FundooContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });

            builder.Services.AddScoped<IUserRL, UserRL>();
            builder.Services.AddScoped<IUserBL, UserBL>();

            builder.Services.AddScoped<INoteRL, NoteRL>();
            builder.Services.AddScoped<INoteBL, NoteBL>();

            builder.Services.AddScoped<ILabelRL, LabelRL>();
            builder.Services.AddScoped<ILabelBL, LabelBL>();

            builder.Services.AddScoped<INoteLabelRL, NoteLabelRL>();
            builder.Services.AddScoped<INoteLabelBL, NoteLabelBL>();

            builder.Services.AddScoped<ICollaboratorRL, CollaboratorRL>();
            builder.Services.AddScoped<ICollaboratorBL, CollaboratorBL>();

            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("getUserPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:5264")
                    .WithMethods("GET")
                    .WithHeaders("*");
                });
                options.AddPolicy("default", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });

            //redis services
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration["RedisCacheOptions:Configuration"];
                options.InstanceName = builder.Configuration["RedisCacheOptions:InstanceName"];
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Fundoo API V1");
                    options.RoutePrefix = string.Empty;
                });
            }


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("getUserPolicy");
            app.UseCors("default");

            app.MapControllers();

            app.Run();
        }
    }
}