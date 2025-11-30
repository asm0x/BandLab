using BandLab.Clients;

var builder = WebApplication.CreateSlimBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Test>();

var app = builder.Build();

app.MapDefaultEndpoints();
app.Run();
