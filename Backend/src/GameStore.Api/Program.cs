
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddHttpLogging(opt =>
{
    opt.LoggingFields =
        HttpLoggingFields.RequestMethod |
        HttpLoggingFields.RequestPath |
        HttpLoggingFields.ResponseStatusCode |
        HttpLoggingFields.Duration;
    opt.CombineLogs = true;
});

var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<KeycloakClaimsTransformer>();

builder.Services.AddAuthentication(Schemes.Keycloak)
    .AddJwtBearer(opt =>
    {
        opt.MapInboundClaims = false;
        opt.TokenValidationParameters.RoleClaimType =
            GameStore.Api.Shared.Authorization.ClaimTypes.Role;
    })
    .AddJwtBearer(Schemes.Keycloak, opt =>
    {
        opt.RequireHttpsMetadata = false;
        opt.Authority = "http://localhost:8080/realms/gamestore";
        opt.Audience = "gamestore-api";
        opt.MapInboundClaims = false;
        opt.TokenValidationParameters.RoleClaimType =
            GameStore.Api.Shared.Authorization.ClaimTypes.Role;
        opt.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
        {
            OnTokenValidated = ctx =>
            {
                var transformer = ctx.HttpContext
                    .RequestServices
                    .GetRequiredService<KeycloakClaimsTransformer>();
                transformer.Transform(ctx);

                return Task.CompletedTask;
            }
        };
    });

builder.AddGameStoreAuthorization();
builder.Services.AddSingleton<IAuthorizationHandler, BasketAuthorizationHandler>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<FileUploader>();

var app = builder.Build();

app.UseStaticFiles();

app.UseAuthorization();

app.MapGames();
app.MapGenres();
app.MapBaskets();

app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}
else
{
    app.UseExceptionHandler();
}

app.UseStatusCodePages();

await app.InitializeDbAsync();

app.Logger.LogInformation(19, "Starting GameStore.Api...");

app.Run();

/*
 * What service lifetime to use for a dbContext?
 * - DbContext is designed to be used as a single Unit of Work.
 * - DbContext created --> entity changes tracked --> save changes --> dispose
 * - DB connections are expensive.
 * - DBContext is not thread-safe.
 * - Increased memory usage due to change tracking.
 *
 * - USE: Scoped service lifetime.
 * - Aligning the context lifetime to the lifetime of the request.
 * - There is only one thread executing each client request at a given time.
 * - Ensure each request gets a separate DbContext instance.
 *
 */
