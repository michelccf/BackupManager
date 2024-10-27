using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BackupManager.Configuration.Interfaces
{
    public interface INavigationService
    {
        void SetCurrentWindow(Window currentWindow);
        void NavigateTo<T>() where T : UserControl;
    }
}
