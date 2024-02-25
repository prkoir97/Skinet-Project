using API.Extensions;
using API.Middleware;
using Core.Entities.Identity;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args); // Configures kestrel web server, resposible for running our web app


builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);


var app = builder.Build();

// Middleware: software that we can use to do something with that request on it's journey in, and out

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");


app.UseSwagger();               // Swagger documents our API endpoints
app.UseSwaggerUI();


app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseAuthentication(); // Authentication always comes before Authorization (HAS to be in order)
app.UseAuthorization();  // 401, if you reverse order

app.MapControllers(); // Where API sends HTTP requests (registers controller endpoints)

using var scope = app.Services.CreateScope(); // Create a scoped service container scope for managing service lifetimes within a boundary
var services = scope.ServiceProvider; // Obtain the service provider from the created scope
var context = services.GetRequiredService<StoreContext>(); // Retrieve the StoreContext service from the service provider
var identityContext = services.GetRequiredService<AppIdentityDbContext>();
var userManager = services.GetRequiredService<UserManager<AppUser>>();
var logger = services.GetRequiredService<ILogger<Program>>(); // Retrieve the ILogger service with the Program category from the service provider

try
{
    await context.Database.MigrateAsync(); // Attempt to migrate the database asynchronously using the StoreContext
    await identityContext.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
    await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during migration"); // Log any exceptions that occur during migration
}


app.Run();