﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using LiveHTS.Core.Interfaces.Services.Clients;
using LiveHTS.Core.Interfaces.Services.Config;
using LiveHTS.Core.Interfaces.Services.Interview;
using LiveHTS.Core.Model.Config;
using LiveHTS.Core.Model.Interview;
using LiveHTS.Core.Model.Subject;
using LiveHTS.Core.Model.Survey;
using LiveHTS.Presentation.DTO;
using LiveHTS.Presentation.Interfaces;
using LiveHTS.Presentation.Interfaces.ViewModel;
using LiveHTS.Presentation.ViewModel.Template;
using LiveHTS.Presentation.ViewModel.Wrapper;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using Newtonsoft.Json;

namespace LiveHTS.Presentation.ViewModel
{
    public class ClientDashboardViewModel:MvxViewModel,IClientDashboardViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IDashboardService _dashboardService;
        private readonly IInterviewService _interviewService;
        private readonly ILookupService _lookupService;
        protected readonly ISettings _settings;

        private Client _client;
        private Client _seletctedRelationShip;
        private List<RelationshipTemplateWrap> _relationships = new List<RelationshipTemplateWrap>();
        private Module _module;
        private List<FormTemplateWrap> _forms;

        private IMvxCommand _manageRegistrationCommand;
        private IMvxCommand _addRelationShipCommand;

       
        private bool _isBusy;
        private EncounterType _defaultEncounterType;

        public Client Client
        {
            get { return _client; }
            set
            {
                _client = value; RaisePropertyChanged(() => Client);
                Relationships = ConvertToRelationshipWrapperClass(Client.Relationships, this);
            }
        }        
        public Client SeletctedRelationShip
        {
            get { return _seletctedRelationShip; }
            set { _seletctedRelationShip = value; RaisePropertyChanged(() => SeletctedRelationShip); }
        }
        public List<RelationshipTemplateWrap> Relationships
        {
            get { return _relationships; }
            set
            {
                _relationships = value;
                RaisePropertyChanged(() => Relationships);
            }
        }
        public Module Module
        {
            get { return _module; }
            set
            {
                _module = value; RaisePropertyChanged(() => Module);
                var forms = Module.Forms.ToList();
                foreach (var form in forms)
                {
                    if (null == DefaultEncounterType)
                    {
                        DefaultEncounterType = _lookupService.GetDefaultEncounterType();
                    }
                    form.DefaultEncounterTypeId = DefaultEncounterType.Id;
                    form.ClientEncounters = _interviewService.LoadEncounters(Client.Id, form.Id).ToList();
                }
                Forms = ConvertToFormWrapperClass(forms, this);
            }
        }
        public List<FormTemplateWrap> Forms
        {
            get { return _forms; }
            set { _forms = value;RaisePropertyChanged(() => Forms); }
        }

        public EncounterType DefaultEncounterType
        {
            get { return _defaultEncounterType; }
            set { _defaultEncounterType = value; RaisePropertyChanged(() => DefaultEncounterType); }
        }

        public IMvxCommand ManageRegistrationCommand
        {
            get
            {
                _manageRegistrationCommand = _manageRegistrationCommand ?? new MvxCommand(ManageRegistration);
                return _manageRegistrationCommand;
            }
        }
        public IMvxCommand AddRelationShipCommand
        {
            get
            {
                _addRelationShipCommand = _addRelationShipCommand ?? new MvxCommand(AddRelationShip);
                return _addRelationShipCommand;
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        public ClientDashboardViewModel(IDashboardService dashboardService, IDialogService dialogService, IInterviewService interviewService, ISettings settings, ILookupService lookupService)
        {
            _dashboardService = dashboardService;
            _dialogService = dialogService;
            _interviewService = interviewService;
            _settings = settings;
            _lookupService = lookupService;
        }

        public override void ViewAppeared()
        {
            //Reload

            Module = _dashboardService.LoadModule();
            if (null != Module)
            {
                var moduleJson = JsonConvert.SerializeObject(Module);
                _settings.AddOrUpdateValue("module", moduleJson);
            }


            var clientJson = _settings.GetValue("client", "");         
            var encounterTypeJson = _settings.GetValue("encountertype", "");

            if (!string.IsNullOrWhiteSpace(clientJson))
            {
                Client = JsonConvert.DeserializeObject<Client>(clientJson);
            }
           
            if (!string.IsNullOrWhiteSpace(encounterTypeJson))
            {
                DefaultEncounterType = JsonConvert.DeserializeObject<EncounterType>(encounterTypeJson);
            }
            
        }

        private void ManageRegistration()
        {
            ShowViewModel<ClientRegistrationViewModel>(new { id = Client.Id });
        }
        private void AddRelationShip()
        {
            ShowViewModel<ClientRelationshipsViewModel>(new {id = Client.Id});
        }
        public void Init(string id)
        {           
            if (string.IsNullOrWhiteSpace(id))
                return;

            Client = _dashboardService.LoadClient(new Guid(id));
            Module = _dashboardService.LoadModule();
            DefaultEncounterType = _lookupService.GetDefaultEncounterType();

            if (null != Client)
            {
                var clientJson = JsonConvert.SerializeObject(Client);
                _settings.AddOrUpdateValue("client", clientJson);

                var clientDto = ClientDTO.Create(Client);
                var clientDtoJson = JsonConvert.SerializeObject(clientDto);
                _settings.AddOrUpdateValue("client.dto", clientDtoJson);
            }

            if (null != Module)
            {
                var moduleJson = JsonConvert.SerializeObject(Module);
                _settings.AddOrUpdateValue("module", moduleJson);
            }
            
            if (null != DefaultEncounterType)
            {
                var encounterTypeJson = JsonConvert.SerializeObject(DefaultEncounterType);
                _settings.AddOrUpdateValue("encountertype", encounterTypeJson);
            }
            
            
            
        }

      
        public void ShowRegistry()
        {
            ShowViewModel<RegistryViewModel>();
        }

        public async void RemoveRelationship(RelationshipTemplate template)
        {
            try
            {
                var result = await _dialogService.ConfirmAction("Are you sure ?", "Remove Relationship");
                if ( result)
                {
                    _dashboardService.RemoveRelationShip(template.Id);
                    Client = _dashboardService.LoadClient(Client.Id);
                }
            }
            catch (Exception e)
            {
                MvxTrace.Error(e.Message);
                _dialogService.Alert(e.Message,"Remove Relationship");
            }
        }

        

        private static List<RelationshipTemplateWrap> ConvertToRelationshipWrapperClass(IEnumerable<ClientRelationship> clientRelationships, ClientDashboardViewModel clientDashboardViewModel)
        {
            List<RelationshipTemplateWrap> list = new List<RelationshipTemplateWrap>();
            foreach (var r in clientRelationships)
            {
                list.Add(new RelationshipTemplateWrap(new RelationshipTemplate(r), clientDashboardViewModel));
            }

            return list;
        }

        private static List<FormTemplateWrap> ConvertToFormWrapperClass(List<Form> forms, ClientDashboardViewModel clientDashboardViewModel)
        {
            List<FormTemplateWrap> list = new List<FormTemplateWrap>();
            foreach (var r in forms)
            {
                 var f = new FormTemplate(r);
                f.Encounters = ConvertToEncounterWrapperClass(r.ClientEncounters, clientDashboardViewModel,f.Display);
                var fw = new FormTemplateWrap(clientDashboardViewModel, f);                
                list.Add(fw);
            }
            return list;
        }

        private static List<EncounterTemplateWrap> ConvertToEncounterWrapperClass(List<Encounter> encounters, ClientDashboardViewModel clientDashboardViewModel, string fDisplay)
        {
            List<EncounterTemplateWrap> list = new List<EncounterTemplateWrap>();
            foreach (var r in encounters)
            {
                list.Add(new EncounterTemplateWrap(clientDashboardViewModel, new EncounterTemplate(r,fDisplay)));
            }
            return list;
        }


        public void StartEncounter(FormTemplate formTemplate)
        {
            var clientEncounterDTO = ClientEncounterDTO.Create(Client.Id, formTemplate);
            var clientEncounterDTOJson = JsonConvert.SerializeObject(clientEncounterDTO);
            _settings.AddOrUpdateValue("client.encounter.dto", clientEncounterDTOJson);

            ShowViewModel<ClientEncounterViewModel>(new
            {
                formId = formTemplate.Id.ToString(),
                mode = "new",
                encounterId = ""
            });
        }

        public void ResumeEncounter(EncounterTemplate encounterTemplate)
        {
            var clientEncounterDTO = ClientEncounterDTO.Create(Client.Id, encounterTemplate);
            var clientEncounterDTOJson = JsonConvert.SerializeObject(clientEncounterDTO);
            _settings.AddOrUpdateValue("client.encounter.dto", clientEncounterDTOJson);

            ShowViewModel<ClientEncounterViewModel>(new
            {
                formId = encounterTemplate.FormId.ToString(),
                mode = "open",
                encounterId = encounterTemplate.Id.ToString()
            });
        }

        public void ReviewEncounter(EncounterTemplate encounterTemplate)
        {
            ResumeEncounter(encounterTemplate);
        }

        public async void DiscardEncounter(EncounterTemplate encounterTemplate)
        {
            try
            {
                var result = await _dialogService.ConfirmAction("Are you sure ?", "Delete this Encounter");
                if (result)
                {
                    _dashboardService.RemoveEncounter(encounterTemplate.Id);
                    Module = _dashboardService.LoadModule();
                }
            }
            catch (Exception e)
            {
                MvxTrace.Error(e.Message);
                _dialogService.Alert(e.Message, "Delete this Encounter");
            }
        }
    }
}