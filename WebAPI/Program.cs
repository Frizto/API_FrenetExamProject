using ApplicationLayer.DependencyInjection;
using InfrastructureLayer.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using System.Reflection;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Init Main FrenetExam WebAPI");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add Logging services to the container.
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add Layered services to the container.
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();
    
    // Register HttpClient
    builder.Services.AddHttpClient();

    // Add API services to the container.
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "FrenetExam.WebAPI",
            Version = "v1",
            Description = "FrenetExam API Authorization with JWT",
        });

        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = HeaderNames.Authorization,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                }, Array.Empty<string>()
            }
    });
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Add the HTTPS middleware to the pipeline.
    app.UseHttpsRedirection();
    app.UseRouting();

    // Add the Authentication middleware to the pipeline.
    app.UseAuthentication();
    app.UseAuthorization();

    // Protect Swagger endpoints
    app.UseWhen(context => context.Request.Path.StartsWithSegments("/swagger"), appBuilder =>
    {
        appBuilder.UseAuthentication();
        appBuilder.UseAuthorization();
        appBuilder.Use(async (context, next) =>
        {
            if (!context.User.Identity!.IsAuthenticated)
            {
                context.Response.StatusCode = 401;
                return;
            }
            await next();
        });
    });

    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FrenetExam API V1");
        c.RoutePrefix = string.Empty; // Optional: Set Swagger UI at the app's root
    });

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    // Catch setup errors
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}



