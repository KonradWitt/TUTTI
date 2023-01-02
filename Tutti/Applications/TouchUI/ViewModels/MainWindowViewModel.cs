﻿using DataService.Models;
using Services.DataService;
using Services.IdentificationDeviceService;
using Services.IdentificationDeviceService.DataContracts;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;
using TouchUI.Models.Enums;

namespace TouchUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IDataService _dataService;
        private IIdentificationDeviceService _idDeviceService;

        private DateTime _currentDateTime;
        private string _cardIdentifcatorToBeSimulated;
        private string _lastIdentificator;
        private IdentificationMode _identificationMode;

        public MainWindowViewModel(IDataService dataService, IIdentificationDeviceService idDeviceService)
        {
            _dataService = dataService;
            _idDeviceService = idDeviceService;
            _idDeviceService.IdentificationOccured += OnIdServiceIdentificationOccured;
            InitializeCommands();
            InitializeDispatcherTimer();
        }

        private void InitializeCommands() 
        {
            SimulateCardIdentificationCommand = new RelayCommand(SimulateCardIdentification);
            SetIdentificationModeToEntryCommand = new RelayCommand(() => IdentificationMode = IdentificationMode.Entry);
            SetIdentificationModeToExitCommand = new RelayCommand(() => IdentificationMode = IdentificationMode.Exit);
            SetIdentificationModeToInfoCommand = new RelayCommand(() => IdentificationMode = IdentificationMode.Info);
        }

        private void InitializeDispatcherTimer()
        {
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += OnDispatcherTimerElapsed;
            dispatcherTimer.Start();
        }

        private void OnDispatcherTimerElapsed(object? sender, EventArgs e)
        {
            CurrentDateTime = DateTime.Now;
        }

        private void SimulateCardIdentification()
        {
            _idDeviceService.SimulateIdentificationEvent(CardIdentifcatorToBeSimulated);
        }

        private void OnIdServiceIdentificationOccured(object sender, IdentificationOccuredEventArgs eventArgs)
        {
            LastIdentificator = eventArgs?.Identifier;
        }

        public DateTime CurrentDateTime
        {
            get
            {
                return _currentDateTime;
            }
            set
            {
                _currentDateTime = value;
                OnPropertyChanged();
            }
        }

        public string CardIdentifcatorToBeSimulated
        {
            get
            {
                return _cardIdentifcatorToBeSimulated;
            }
            set
            {
                _cardIdentifcatorToBeSimulated= value;
                OnPropertyChanged();
            }
        }    

        public string LastIdentificator
        {
            get
            {
                return _lastIdentificator;
            }
            set
            {
                _lastIdentificator= value;
                OnPropertyChanged();
            }
        }

        public IdentificationMode IdentificationMode
        {
            get
            {
                return _identificationMode;
            }
            set
            {
                _identificationMode= value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsIdentificationModeEntry));
                OnPropertyChanged(nameof(IsIdentificationModeExit));
                OnPropertyChanged(nameof(IsIdentificationModeInfo));
            }
        }

        public bool IsIdentificationModeEntry
        {
            get
            {
                return IdentificationMode == IdentificationMode.Entry;
            }
        }

        public bool IsIdentificationModeExit
        {
            get
            {
                return IdentificationMode == IdentificationMode.Exit;
            }
        }

        public bool IsIdentificationModeInfo
        {
            get
            {
                return IdentificationMode == IdentificationMode.Info;
            }
        }

        public ICommand SimulateCardIdentificationCommand { get; set; }
        public ICommand SetIdentificationModeToEntryCommand { get; set; }
        public ICommand SetIdentificationModeToExitCommand { get; set; }
        public ICommand SetIdentificationModeToInfoCommand { get; set; }


    }
}
