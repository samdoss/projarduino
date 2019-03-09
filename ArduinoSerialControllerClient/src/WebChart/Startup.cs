using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SignalRDemoApp.Hubs;

namespace SignalRDemoApp
{
    public class Startup
    {
       // private IMyFileWatcher _watcher;
		private IMyDataReceiver _receiver;
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddScoped<IMyFileWatcher,MyFileWatcher>();
			services.AddScoped<IMyDataReceiver, MyDataReceiver>();
			services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMyDataReceiver receiver)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //_watcher = watcher;
			_receiver = receiver;

            app.UseFileServer();

            app.UseSignalR(routes =>
            {
               // routes.MapHub<FileWatcherHub>("/fileWatcherHub");

				routes.MapHub<ReceiveDataHub>("/receiveDataHub");
			});

			app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
