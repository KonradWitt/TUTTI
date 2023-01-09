﻿using DataService.Models;
using Serilog;
using Services.DataService;
using Services.IdentificationDeviceService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TouchUI.Services.Login;
using TouchUI.Services.Navigation;
using TouchUI.Tools.Navigation;

namespace TouchUI.ViewModels
{
    public class HistoryViewModel : NavigationViewModelBase
    {
        private readonly ILogger _logger = Log.Logger.ForContext<HomeViewModel>();
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private readonly ILoginService _loginService;

        private User _currentUser;
        private ObservableCollection<NavigationTarget> _navigatableViewModels = new ObservableCollection<NavigationTarget>{
            new NavigationTarget(typeof(HomeViewModel), "Home", true),
            new NavigationTarget(typeof(ExitViewModel), "Finish work", true) };
        private ObservableCollection<TimeStamp> _timeStampsHistory = new ObservableCollection<TimeStamp>();

        public HistoryViewModel(IDataService dataService,
            IIdentificationDeviceService idDeviceService,
            INavigationService navigationService,
            ILoginService loginService)
            : base(navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            _loginService = loginService;
            _currentUser = _loginService.GetCurrentUser();
            InitializeHistory();
        }

        private void InitializeHistory()
        {
            if (_currentUser == null)
            {
                _logger.Error("Current user was null when trzing to initialize timestamps history.");
                return;
            }
            _timeStampsHistory.Clear();
            var oldestTimeStampDate = DateTime.Now - TimeSpan.FromDays(30);
            var newestTimeStampDate = DateTime.Now;
            var timeStampHistory = _dataService.GetTimeStamps(_currentUser.Id, oldestTimeStampDate, newestTimeStampDate).OrderByDescending(x => x.DateTime);
            TimeStampsHistory = new ObservableCollection<TimeStamp>(timeStampHistory);
        }

        public ObservableCollection<TimeStamp> TimeStampsHistory
        {
            get
            {
                return _timeStampsHistory;
            }
            set
            {
                _timeStampsHistory = value;
                OnPropertyChanged();
            }
        }

        public override ObservableCollection<NavigationTarget> NavigatableViewModels
        {
            get
            {
                return _navigatableViewModels;
            }
            set
            {
                _navigatableViewModels = value;
                OnPropertyChanged();
            }
        }

        public override void Uninitialize()
        {

        }
    }
}