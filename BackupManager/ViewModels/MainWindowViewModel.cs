using BackupManager.Util;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BackupManager.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IServiceProvider _serviceProvider;
        public ICommand AbrirConfiguracoesCommand { get; }  
        public ICommand ExecutarAcaoCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            AbrirConfiguracoesCommand = new RelayCommand(AbrirConfiguracoes);
            ExecutarAcaoCommand = new RelayCommand(ExecutarAcao);
        }

        private void AbrirConfiguracoes()
        {
            var configuracoesWindow = _serviceProvider.GetRequiredService<Views.Configuration>();
            configuracoesWindow.Show();
        }

        private void ExecutarAcao()
        {
            // Lógica para executar a ação
            Console.WriteLine("Ação executada!");
        }

        
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
