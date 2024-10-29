using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using BackupManager.Models;
using BackupManager.Util;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using BackupManager.Views;
using BackupManager.Configuration.Interfaces;
using System.Windows.Threading;
using System.Windows;

namespace BackupManager.ViewModels
{
    public class ConfigurationViewModel : INotifyPropertyChanged
    {

        public ICommand AddGameCommand { get; }
        public ICommand SelectGamePathCommand { get; }
        public ICommand SelectBackupPathCommand { get; }
        public ICommand ReturnCommand { get; set; }
        public ICommand DeleteIndexCommand { get; set; }
        public ICommand DefineTimeCommand { get; set; }

        private readonly INavigationService _navigationService;

        public ObservableCollection<Game> Games { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private string _newName;
        private string _newGamePath;
        private string _newBackupPath;
        private bool _horas;
        private bool _minutos;
        private bool _segundos;
        private int _tempo;
        private Game _selectedGame;

        public bool Horas
        {
            get { return _horas; }
            set
            {
                _horas = value;
                OnPropertyChanged(nameof(Horas));
            }
        }

        public bool Minutos
        {
            get { return _minutos; }
            set
            {
                _minutos = value;
                OnPropertyChanged(nameof(Minutos));
            }
        }

        public bool Segundos
        {
            get { return _segundos; }
            set
            {
                _segundos = value;
                OnPropertyChanged(nameof(Segundos));
            }
        }

        public int Tempo
        {
            get { return _tempo; }
            set
            {
                _tempo = value;
                OnPropertyChanged(nameof(Tempo));
            }
        }

        public string NewName
        {
            get { return _newName; }
            set
            {
                _newName = value;
                OnPropertyChanged(nameof(NewName));
            }
        }

        public string NewGamePath
        {
            get { return _newGamePath; }
            set
            {
                _newGamePath = value;
                OnPropertyChanged(nameof(NewGamePath));
            }
        }

        public string NewBackupPath
        {
            get { return _newBackupPath; }
            set
            {
                _newBackupPath = value;
                OnPropertyChanged(nameof(NewBackupPath));
            }
        }

        public Game SelectedIndex
        {
            get { return _selectedGame; }
            set
            {
                _selectedGame = value;
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }

        public ConfigurationViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            Games = new ObservableCollection<Game>();
            AddGameCommand = new RelayCommand(ExecuteActionAddGame);
            SelectGamePathCommand = new RelayCommand(ExecuteActionSelectGamePath);
            SelectBackupPathCommand = new RelayCommand(ExecuteActionSelectBackupPath);
            ReturnCommand = new RelayCommand(ExecuteActionReturn);
            DeleteIndexCommand = new RelayCommand(ExecuteActionDeleteIndex);
            DefineTimeCommand = new RelayCommand(ExecuteActionDefineTime);
            GetPropertiesFromJsonConfig();
            if (!Horas && !Minutos && !Segundos)
            {
                Horas = true;
            }

        }

        private void ExecuteActionDefineTime()
        {
            DefineTimeOnly();
        }

        private void ExecuteActionDeleteIndex()
        {
            if (SelectedIndex != null)
            {
                Games.Remove(SelectedIndex);
                GenerateJson();
                SendMessageBox("Elemento removido com sucesso!");
            }
            else 
            {
                SendMessageBox("Clique em um elemento da lista!");
            }
        }

        private void ExecuteActionReturn()
        {
            _navigationService.NavigateTo<MainPanel>();
        }

        private void ExecuteActionSelectBackupPath()
        {
                NewBackupPath = OpenFileDialog();
        }

        public void ExecuteActionAddGame()
        {
            if (string.IsNullOrEmpty(NewName) || string.IsNullOrEmpty(NewGamePath) || string.IsNullOrEmpty(NewBackupPath))
            {
                SendMessageBox("Defina o nome, caminho do arquivo origem e o caminho destino.");
                return;
            }

            if (Tempo == 0 || Tempo == null)
            {
                SendMessageBox("Defina o valor de tempo.");
                return;
            }
            

            Game x = new Game();
            x.Name = NewName;
            x.Path = NewGamePath;
            x.Pathbackup = NewBackupPath;
            Games.Add(x);

            NewName = string.Empty;
            NewGamePath = string.Empty;
            NewBackupPath = string.Empty;
            GenerateJson();
            SendMessageBox("Item adicionado com sucesso!");
        }

        public void ExecuteActionSelectGamePath()
        {
            NewGamePath = OpenFileDialog();
        }

        private string OpenFileDialog()
        {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "Selecione a pasta";
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = "C:\\";
            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = "C:\\";
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                return dlg.FileName;

            else
                return string.Empty;
        }

        private void GetPropertiesFromJsonConfig()
        {
            JsonConfig json = DesserializerJsonConfig();
            if (json != null)
            {
                Games = new ObservableCollection<Game>(json.Games);
                Horas = json.Horas == true ? true : false;
                Minutos = json.Minutos == true ? true : false;
                Segundos = json.Segundos == true ? Segundos = true : false;
                Tempo = json.Tempo;
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

        private void DefineTimeOnly()
        {
            JsonConfig json = DesserializerJsonConfig();
            json.Horas = Horas;
            json.Minutos = Minutos;
            json.Segundos = Segundos;
            json.Tempo = Tempo;
            GenerateJson(json);
            SendMessageBox("Tempo definido com sucesso!");
        }

        private void GenerateJson(JsonConfig jsonConfig = null)
        {
            if (jsonConfig == null)
            {
                jsonConfig = new JsonConfig();
                jsonConfig.Games = Games.ToList();
                string json = JsonConvert.SerializeObject(jsonConfig, Formatting.Indented);
                // Salva o JSON em um arquivo
                File.WriteAllText(Constants.JsonPath, json);
            }
            else 
            {
                File.WriteAllText(Constants.JsonPath, JsonConvert.SerializeObject(jsonConfig, Formatting.Indented));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SendMessageBox(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
