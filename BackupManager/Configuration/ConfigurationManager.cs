using BackupManager.Configuration.Interfaces;
using BackupManager.ViewModels;
using BackupManager.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupManager.Configuration
{
    public class ConfigurationManager
    {

        public IServiceProvider ServiceProvider { get; private set; }
        public ConfigurationManager() 
        {
            ServiceBuild();
        }

        private void ContainerFactory(IServiceCollection services)
        {
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddTransient<Views.MainWindow>();
            services.AddTransient<Views.Configuration>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<ConfigurationViewModel>();
        }

        public IServiceProvider ServiceBuild()
        {
            ServiceCollection ServicesCollection = new ServiceCollection();
            ContainerFactory(ServicesCollection);
            ServiceProvider = ServicesCollection.BuildServiceProvider();

            return ServiceProvider;
        }
    }
}
