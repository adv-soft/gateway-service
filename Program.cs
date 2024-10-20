using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
// builder.WebHost.UseUrls("https://localhost:443");
// builder.WebHost.ConfigureKestrel(options =>
// {
//     options.ListenAnyIP(443, listenOptions =>
//     {
//         listenOptions.UseHttps();
//     });
// });
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();
builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddCors();

// Add Swagger
// builder.Services.AddMvcCore();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo() { Title = "API Gateway", Version = "v1" });
// });

var app = builder.Build();
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// app.UseSwagger();
// app.UseSwaggerUI(c =>
// {
//     c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthService API v1");
//     c.RoutePrefix = string.Empty; // Launch at the root
// });


await app.UseOcelot();
app.Run();