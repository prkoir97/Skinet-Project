using API.Extensions;
using API.Middleware;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args); // Configures kestrel web server, resposible for running our web app


builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);


var app = builder.Build();

// Middleware: software that we can use to do something with that request on it's journey in, and out

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");


app.UseSwagger();               // Swagger documents our API endpoints
app.UseSwaggerUI();


app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers(); // Where API sends HTTP requests (registers controller endpoints)

using var scope = app.Services.CreateScope(); // Create a scoped service container scope for managing service lifetimes within a boundary
var services = scope.ServiceProvider; // Obtain the service provider from the created scope
var context = services.GetRequiredService<StoreContext>(); // Retrieve the StoreContext service from the service provider
var logger = services.GetRequiredService<ILogger<Program>>(); // Retrieve the ILogger service with the Program category from the service provider

try
{
    await context.Database.MigrateAsync(); // Attempt to migrate the database asynchronously using the StoreContext
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during migration"); // Log any exceptions that occur during migration
}


app.Run();