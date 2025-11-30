using BandLab.Files;
using BandLab.Scaling.Queue;

namespace BandLab.CDN;

public class App
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);

        builder.AddServiceDefaults();
        builder.Services
            .AddFileStorage(sp => sp.GetRequiredService<IWebHostEnvironment>().WebRootPath)
            .AddScaleEvents<Uploader>(builder.Configuration.GetSection("ScaleQueue"),
                options => options.Connection ??= builder.Configuration["ConnectionStrings:queues"]);

        var app = builder.Build();

        app.MapDefaultEndpoints();

        app.UseStaticFiles();
        
        app.Run(async context =>
        {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Image is compressing, please check later");
        });
        
        app.Run();
    }
}