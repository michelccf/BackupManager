using BackupManager.Configuration.Interfaces;
using BackupManager.Util;
using BackupManager.Views;
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
    public class MainPanelViewModel : INotifyPropertyChanged
    {
        private readonly Views.Configuration configurationView;

        private readonly INavigationService _navigationService;
        public ICommand AbrirConfiguracoesCommand { get; }
        public ICommand ExecutarAcaoCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainPanelViewModel(IServiceProvider serviceProvider, INavigationService navigationService, Views.Configuration _configurationView)
        {
            configurationView = _configurationView;
            _navigationService = navigationService;
            AbrirConfiguracoesCommand = new RelayCommand(AbrirConfiguracoes);
            ExecutarAcaoCommand = new RelayCommand(ExecutarAcao);
        }

        private void AbrirConfiguracoes()
        {
            _navigationService.NavigateTo<Views.Configuration>();
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
