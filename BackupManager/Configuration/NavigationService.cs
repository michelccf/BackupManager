using BackupManager.Configuration.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BackupManager.Configuration
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private Window _currentWindow;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void SetCurrentWindow(Window currentWindow)
        {
            _currentWindow = currentWindow;
        }

        public void NavigateTo<T>() where T : UserControl
        {
            var newWindow = _serviceProvider.GetRequiredService<T>();
            _currentWindow?.Close();
        }
    }
}
