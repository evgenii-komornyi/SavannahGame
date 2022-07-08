using GameEngine.Services.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UI;

var host = CreateHostBuilder(args).Build();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
host.Services.GetService<IGameManager>().RunApplication();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

static IHostBuilder CreateHostBuilder(string[] args)
{
    var hostBuilder = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, builder) =>
        {
            builder.SetBasePath(Directory.GetCurrentDirectory());
        })
        .ConfigureServices((context, services) =>
        {
            services.AddSingleton<IGameManager, GameManager>();
            services.AddSingleton<IUserInterface, UserInterface>();
            services.AddSingleton<IWindow, Window>();
            services.AddSingleton<IObjectManager, ObjectManager>();
            services.AddSingleton<IPairManager, PairManager>();
            services.AddSingleton<IMovementManager, MovementManager>();

        });

    return hostBuilder;
}