using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BackupManager.Models;
using BackupManager.Util;
using Microsoft.Win32;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Dynamic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using BackupManager.Views;
using Microsoft.Extensions.DependencyInjection;


namespace BackupManager.ViewModels
{
    public class ConfigurationViewModel : INotifyPropertyChanged
    {

        public ICommand AddGameCommand { get; }
        public ICommand SelectGamePathCommand { get; }
        public ICommand SelectBackupPathCommand { get; }
        public ICommand ReturnCommand { get; set; }

        private readonly IServiceProvider _serviceProvider;

        public ObservableCollection<Game> Games { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private string _newName;
        private string _newGamePath;
        private string _newBackupPath;

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

        public ConfigurationViewModel(IServiceProvider serviceProvider)
        {
            Games = new ObservableCollection<Game>();
            AddGameCommand = new RelayCommand(ExecuteActionAddGame);
            SelectGamePathCommand = new RelayCommand(ExecuteActionSelectGamePath);
            SelectBackupPathCommand = new RelayCommand(ExecuteActionSelectBackupPath);
            ReturnCommand = new RelayCommand(ExecuteActionReturn);
            _serviceProvider = serviceProvider;
            GetPropertiesFromJsonConfig();

        }

        private void ExecuteActionReturn()
        {
            MainWindow _mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            _mainWindow.Show();
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
            if (File.Exists(Constants.JsonPath)) 
            { 
                string json = File.ReadAllText(Constants.JsonPath);

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

            // Salva o JSON em um arquivo
            File.WriteAllText(Constants.JsonPath, json);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
