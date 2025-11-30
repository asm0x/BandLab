namespace BandLab.API.Models;

public record CreatePostModel(string? Caption, IFormFile? Image)
{
    private readonly string[] Allowed = [".jpg", ".jpeg", ".png", ".bmp"];

    public static async ValueTask<CreatePostModel?> BindAsync(HttpContext context) =>
        (await Read(context))
            .Validate();

    private static async Task<CreatePostModel> Read(HttpContext context) =>
        Map(await context.Request.ReadFormAsync());

    private static CreatePostModel Map(IFormCollection data) =>
        new(Caption: data["Caption"],
            Image: data.Files["Image"]);

    private CreatePostModel? Validate() =>
        string.IsNullOrWhiteSpace(Caption) && Image is null
            ? throw new ValidationException("At least Caption or Image should be defined")
            : Image is not null
                ? !Allowed.Contains(Path.GetExtension(Image.FileName), StringComparer.InvariantCultureIgnoreCase)
                    ? throw new ValidationException("Image format is not allowed")
                    : this
                : this;
}
