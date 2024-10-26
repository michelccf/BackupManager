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


namespace BackupManager.ViewModels
{
    public class ConfigurationViewModel : INotifyPropertyChanged
    {

        public ICommand AddGameCommand { get; }
        public ICommand SelectGamePathCommand { get; }
        public ICommand SelectBackupPathCommand { get; }

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

        public ConfigurationViewModel()
        {
            Games = new ObservableCollection<Game>();
            AddGameCommand = new RelayCommand(ExecuteActionAddGame);
            SelectGamePathCommand = new RelayCommand(ExecuteActionSelectGamePath);
            SelectBackupPathCommand = new RelayCommand(ExecuteActionSelectBackupPath);
        }

        private void ExecuteActionSelectBackupPath()
        {
            throw new NotImplementedException();
        }

        public void ExecuteActionAddGame()
        {
            if(NewName == string.Empty || NewGamePath == string.Empty)
             return; 

            Game x = new Game();
            x.Name = NewName;
            x.Path = NewGamePath;
            Games.Add(x);

            NewName = string.Empty;
            NewGamePath = string.Empty;
        }

        public void ExecuteActionSelectGamePath()
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
            {
                NewGamePath = dlg.FileName;
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
