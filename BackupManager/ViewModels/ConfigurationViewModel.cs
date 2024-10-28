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


namespace BackupManager.ViewModels
{
    public class ConfigurationViewModel : INotifyPropertyChanged
    {

        public ICommand AddGameCommand { get; }
        public ICommand SelectGamePathCommand { get; }
        public ICommand SelectBackupPathCommand { get; }
        public ICommand ReturnCommand { get; set; }
        public ICommand DeleteIndexCommand { get; set; }

        private readonly INavigationService _navigationService;

        public ObservableCollection<Game> Games { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private string _newName;
        private string _newGamePath;
        private string _newBackupPath;
        private Game _selectedGame;

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
            GetPropertiesFromJsonConfig();

        }

        private void ExecuteActionDeleteIndex()
        {
            if (SelectedIndex != null)
            {
                Games.Remove(SelectedIndex);
                GenerateJson();
            }
        }

        private void ExecuteActionReturn()
        {
            _navigationService.NavigateTo<MainPanel>();
        }

        private void ExecuteActionSelectBackupPath()
        {
                NewBackupPath = OpenFileDialog();
                GenerateJson();
        }

        public void ExecuteActionAddGame()
        {
            if(string.IsNullOrEmpty(NewName)  || string.IsNullOrEmpty(NewGamePath))
             return; 

            Game x = new Game();
            x.Name = NewName;
            x.Path = NewGamePath;
            Games.Add(x);

            NewName = string.Empty;
            NewGamePath = string.Empty;
            GenerateJson();
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
                NewBackupPath = json.BackupPath;
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

        private void GenerateJson()
        {
            JsonConfig jsonConfig = new JsonConfig();
            jsonConfig.Games = Games.ToList();
            jsonConfig.BackupPath = NewBackupPath;
            string json = JsonConvert.SerializeObject(jsonConfig, Formatting.Indented);

            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').LastOrDefault();

            // Salva o JSON em um arquivo
            File.WriteAllText(Constants.JsonPath.Replace("%User%", userName), json);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
