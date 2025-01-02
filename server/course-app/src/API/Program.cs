using Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;
var configuration = builder.Configuration;
var appSettings = configuration.GetSection("AppSettings").Get<API.Settings.AppSettings>();

configuration
    .SetBasePath(env.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

builder.Services
    .AddApplication()
    .AddInfrastructure(configuration);

builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(b =>
    {
        if (appSettings.Cors?.Origins?.Any() == true) b.WithOrigins(appSettings.Cors.Origins);
        else b.AllowAnyOrigin();

        if (appSettings.Cors?.Methods?.Any() == true) b.WithMethods(appSettings.Cors.Methods);
        else b.AllowAnyMethod();

        if (appSettings.Cors?.Headers?.Any() == true) b.WithHeaders(appSettings.Cors.Headers);
        else b.AllowAnyHeader();
    });
});

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.Seed();

app.Run();
