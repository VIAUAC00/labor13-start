using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(args)
    .ConfigureServices(services => services.AddSingleton<GuessWS.GameStateService>())
    .Configure(app => app.UseDefaultFiles()
                         .UseStaticFiles()
                         .UseWebSockets()
                         .UseMiddleware<GuessWS.WebSocketServerMiddleware>())
    .Build()
    .Run();
