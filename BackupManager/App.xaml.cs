using BackupManager.Configuration.Interfaces;
using BackupManager.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using ConfigurationManager = BackupManager.Configuration.ConfigurationManager;

namespace BackupManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            ConfigurationManager config = new ConfigurationManager();
            ServiceProvider = config.ServiceBuild();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            //mainWindow.SetCurrentWindow(MainWindow);
            mainWindow.Show();
        }
    }
}
