using ApplicationLayer.Services.User;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace MAUI;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

        // Register HttpClient with a base address
        builder.Services.AddHttpClient<IUserAPIService, UserAPIService>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7124/");
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
        //.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        //{
        //    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        //});
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
