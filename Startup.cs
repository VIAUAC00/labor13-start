using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SpaServices.Webpack;

namespace GuessWS
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<GameStateService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions { HotModuleReplacement = true });

            app.UseWebSockets();

            app.UseMiddleware<WebSocketServerMiddleware>();

            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
