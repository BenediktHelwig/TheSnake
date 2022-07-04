using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TheSnake.Classes;

namespace TheSnake
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddTransient<Snake>();
            services.AddTransient<Food>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
