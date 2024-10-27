﻿using BackupManager.Util;
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
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly Views.Configuration configurationView;
        
        private readonly IServiceProvider _serviceProvider;
        public ICommand AbrirConfiguracoesCommand { get; }  
        public ICommand ExecutarAcaoCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel(IServiceProvider serviceProvider, Views.Configuration _configurationView)
        {
            _serviceProvider = serviceProvider;
            configurationView = _configurationView;
            AbrirConfiguracoesCommand = new RelayCommand(AbrirConfiguracoes);
            ExecutarAcaoCommand = new RelayCommand(ExecutarAcao);
        }

        private void AbrirConfiguracoes()
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Close();

            configurationView.Show();
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
