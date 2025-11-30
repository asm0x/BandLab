using BandLab.Repositories;

namespace BandLab.API;

public static class AccountDelete
{
    public static WebApplication UseAccountDelete(this WebApplication app)
    {
        app.MapDelete("/account",
            async (HttpContext context, IAccountsRepository accounts, ILogger<App> log) =>
            {
                var account = await accounts.GetById(context.UserId());
                if (account is null)
                    return Results.NotFound();

                await accounts.DeleteAsync(account);

                return Results.NoContent();
            })
            .RequireAuthorization();

        return app;
    }
}
