using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BackupManager.Models;
using BackupManager.Util;

namespace BackupManager.ViewModels
{
    public class ConfigurationViewModel : INotifyPropertyChanged
    {

        public ICommand AddGameCommand { get; }
        public List<Game> Games { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public ConfigurationViewModel()
        {
            AddGameCommand = new RelayCommand(ExecuteActionAddGame);
        }
        public void ExecuteActionAddGame()
        {
            //Implementar Função IO
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
