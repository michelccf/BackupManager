using BackupManager.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BackupManager.ViewModels
{
    internal class MainWindowViewModel
    {
        public ICommand AbrirConfiguracoesCommand { get; }
        public ICommand ExecutarAcaoCommand { get; }

        public MainWindowViewModel()
        {
            AbrirConfiguracoesCommand = new RelayCommand(AbrirConfiguracoes);
            ExecutarAcaoCommand = new RelayCommand(ExecutarAcao);
        }

        private void AbrirConfiguracoes()
        {
            // Lógica para abrir a janela de configurações
            //ConfiguracoesWindow configuracoesWindow = new ConfiguracoesWindow();
            //configuracoesWindow.Show();
        }

        private void ExecutarAcao()
        {
            // Lógica para executar a ação
            Console.WriteLine("Ação executada!");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
