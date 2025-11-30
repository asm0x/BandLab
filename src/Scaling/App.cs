using BandLab.Files;
using BandLab.Scaling;
using BandLab.Scaling.Queue;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services
    .AddScaleProcessing<Scaler>(builder.Configuration.GetSection("ScaleQueue"),
        options => options.Connection ??= builder.Configuration["ConnectionStrings:queues"])
    .AddFileStorage();

builder.Services.AddSingleton<Probes>();

var app = builder.Build();

app.MapDefaultEndpoints();

app.Run();
