using BandLab.Files;
using BandLab.Persistence.MongoDB;
using BandLab.Persistence.Sqlite;
using BandLab.Scaling.Queue;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Text.Json.Serialization;

namespace BandLab.API;

public class App
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);

        builder.AddServiceDefaults();

        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, ModelsSerialization.Default);
        });

        builder.Services
            .AddAntiforgery()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Key"]
                        ?? throw new InvalidOperationException("Secret key is not defined in configuration")))
                };
            });

        builder.Services.AddValidation();

        builder.Services.AddAuthorization();

        builder.Services
            .AddSqliteRepositories(builder.Configuration.GetSection("Sqlite"))
            /*
            .AddMongoDBRepositories(builder.Configuration.GetSection("MongoDB"),
                options => options.Connection = builder.Configuration["ConnectionStrings:db"] ?? options.Connection)
            */
            .AddScaleTasks(builder.Configuration.GetSection("ScaleQueue"),
                options => options.Connection ??= builder.Configuration["ConnectionStrings:queues"])
            .AddFileStorage();

        builder.Services.AddExceptionHandler<GuardExceptions>();
        builder.Services.AddProblemDetails();

        var app = builder.Build();

        app.UseExceptionHandler()
            .UseAntiforgery();

        app.MapDefaultEndpoints()
            .UsePostCreate()
            .UsePostGet()
            .UsePosts()
            .UseCommentCreate()
            .UseCommentGet()
            .UseAccountDelete();

        app.Run();
    }
}