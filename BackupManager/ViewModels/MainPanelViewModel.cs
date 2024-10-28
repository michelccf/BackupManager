using BackupManager.Configuration.Interfaces;
using BackupManager.Models;
using BackupManager.Util;
using BackupManager.Views;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
            JsonConfig json = DesserializerJsonConfig();

            if(json != null && json.Games != null)
                for (int i = 0; i < json.Games.Count; i++)
                {
                    File.Copy($"{json.Games[i].Path}\\{json.Games[i]}", json.BackupPath);
                }
            
        }
        private JsonConfig DesserializerJsonConfig()
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').LastOrDefault();
            if (File.Exists(Constants.JsonPath.Replace("%User%", userName)))
            {

                string json = File.ReadAllText(Constants.JsonPath.Replace("%User%", userName));

                JsonConfig objeto = JsonConvert.DeserializeObject<JsonConfig>(json);
                return objeto;
            }

            return null;

        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
