using Microsoft.AspNetCore.Builder;

namespace EGeek.Id.Config;

public static class IdConfigApp
{
    public static void Apply(WebApplication app)
    {
        app.MapPost("/v1/users", PostUserUseCase.Action);
    }
}