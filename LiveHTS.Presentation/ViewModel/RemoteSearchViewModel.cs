﻿using System.Collections.Generic;
using System.Linq;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using LiveHTS.Core.Interfaces.Services.Clients;
using LiveHTS.Core.Interfaces.Services.Config;
using LiveHTS.Core.Interfaces.Services.Sync;
using LiveHTS.Core.Model.Config;
using LiveHTS.Core.Model.Subject;
using LiveHTS.Presentation.Interfaces;
using LiveHTS.Presentation.Interfaces.ViewModel;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Newtonsoft.Json;

namespace LiveHTS.Presentation.ViewModel
{
    public class RemoteSearchViewModel:MvxViewModel,IRemoteSearchViewModel
    {
        private readonly ISettings _settings;
        private readonly IRemoteSearchService _remoteSearchService;
        private readonly IDeviceSetupService _deviceSetupService;
        private readonly IDialogService _dialogService;
        private readonly IChohortClientsSyncService _chohortClientsSyncService;
        private readonly IClientSyncService _clientSyncService;
        private IEnumerable<Client> _clients;
        private bool _isBusy;
        private string _search;
        private IMvxCommand _searchCommand;
        private IMvxCommand _clearSearchCommand;
        private Client _selectedClient;
        private IMvxCommand<Client> _clientSelectedCommand;

        public Device Device { get; set; }
        public ServerConfig Local { get; set; }
        public string Address { get; set; }
        public string Title { get; set; } = "Remote Search";
        public IRemoteRegistryViewModel Parent { get; set; }

        public string Search
        {
            get { return _search; }
            set
            {
                _search = value; 
                RaisePropertyChanged(() => Search);
                SearchCommand.RaiseCanExecuteChanged();
            }
        }

        public Client SelectedClient
        {
            get { return _selectedClient; }
            set { _selectedClient = value; RaisePropertyChanged(() => SelectedClient);}
        }

        public IEnumerable<Client> Clients
        {
            get { return _clients; }
            set
            {
                _clients = value;
                RaisePropertyChanged(() => Clients);
            }
        }

        public IMvxCommand SearchCommand
        {
            get
            {
                _searchCommand = _searchCommand ?? new MvxCommand(SearchClients, CanSearch);
                return _searchCommand;
            }
        }

        public IMvxCommand ClearSearchCommand
        {
            get
            {
                _clearSearchCommand = _clearSearchCommand ?? new MvxCommand(ClearSearch);
                return _clearSearchCommand;
            }
        }

        public IMvxCommand<Client> ClientSelectedCommand
        {
            get
            {
                _clientSelectedCommand = _clientSelectedCommand ?? new MvxCommand<Client>(SelectClient);
                return _clientSelectedCommand;
            }
        }

     

      

        private void DeletePerson()
        {
            throw new System.NotImplementedException();
        }

       
        private void SelectClient(Client selectedClient)
        {
            _dialogService.Alert("Downloads are currently not enabled !");
            return;

            if (null==selectedClient)
                return;
            SelectedClient = selectedClient;
            ShowViewModel<DashboardViewModel>(new {id = SelectedClient.Id});
        }


        private void ClearSearch()
        {
            Search = string.Empty;
            LoadClients();
        }

        private async void SearchClients()
        {
            IsBusy = true;
            _dialogService.ShowWait("Searching,Please wait...");

            var remoteData = await _clientSyncService.SearchClients(Address, Search);
            if (remoteData.Count > 0)
            {
                Clients = remoteData.Select(x => x.Client).ToList();
            }
            else
            {
                Clients=new List<Client>();
                _dialogService.HideWait();
                _dialogService.ShowToast("No clients found!");
                IsBusy = false;return;
            }
            _dialogService.HideWait();
            IsBusy = false;
        }
        private bool CanSearch()
        {
            return !string.IsNullOrWhiteSpace(Search) && Search.Length > 0;
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value; 
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public RemoteSearchViewModel()
        {
            _clientSyncService = Mvx.Resolve<IClientSyncService>();
            _chohortClientsSyncService = Mvx.Resolve<IChohortClientsSyncService>(); ;
            _deviceSetupService = Mvx.Resolve<IDeviceSetupService>();
            _dialogService = Mvx.Resolve<IDialogService>();
            _remoteSearchService = Mvx.Resolve<IRemoteSearchService>();
            _settings = Mvx.Resolve<ISettings>();
        }

        public void Init()
        {
            LoadInit();
        }

        public override void ViewAppeared()
        {
            LoadInit();
        }

        private void LoadInit()
        {
            var deviceJson = _settings.GetValue("device.id", "");
            var hapiLocal = _settings.GetValue("hapi.local", "");

            if (null == Device && !string.IsNullOrWhiteSpace(deviceJson))
            {
                Device = JsonConvert.DeserializeObject<Device>(deviceJson);
            }
            else
            {
                Device = _deviceSetupService.GetDefault();
                if (null == Device)
                {
                    Device = new Device();
                }
                else
                {
                    var json = JsonConvert.SerializeObject(Device);
                    _settings.AddOrUpdateValue("device.id", json);
                }
            }

            if (null == Local && !string.IsNullOrWhiteSpace(hapiLocal))
            {
                Local = JsonConvert.DeserializeObject<ServerConfig>(hapiLocal);
            }
            else
            {
                Local = _deviceSetupService.GetLocal();
                if (null == Local)
                {
                    Local = new ServerConfig();
                }
                else
                {
                    var json = JsonConvert.SerializeObject(Local);
                    _settings.AddOrUpdateValue("hapi.local", json);
                }
            }
            Address = Local.Address;
        }

      

        private void LoadClients()
        {
            IsBusy = true;
            Clients =new List<Client>();
            IsBusy = false;
        }

       
    }
}