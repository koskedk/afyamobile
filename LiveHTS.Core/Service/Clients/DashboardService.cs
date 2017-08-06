﻿using System;
using System.Collections.Generic;
using System.Linq;
using LiveHTS.Core.Interfaces.Repository.Subject;
using LiveHTS.Core.Interfaces.Repository.Survey;
using LiveHTS.Core.Interfaces.Services.Clients;
using LiveHTS.Core.Model.Subject;
using LiveHTS.Core.Model.Survey;
using LiveHTS.SharedKernel.Custom;

namespace LiveHTS.Core.Service.Clients
{
    public class DashboardService:IDashboardService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IClientRelationshipRepository _clientRelationshipRepository;
        private readonly IModuleRepository _moduleRepository;

        public DashboardService(IClientRepository clientRepository, IClientRelationshipRepository clientRelationshipRepository, IModuleRepository moduleRepository)
        {
            _clientRepository = clientRepository;
            _clientRelationshipRepository = clientRelationshipRepository;
            _moduleRepository = moduleRepository;
        }

        public Client LoadClient(Guid clientId)
        {
            var client= _clientRepository.Get(clientId);

            if (null != client)
                client.Relationships = _clientRelationshipRepository.GetRelationships(clientId).ToList();

            return client;
        }
        public Module LoadModule()
        {
            return _moduleRepository.GetDefaultModule();
        }
        public void RemoveRelationShip(Guid id)
        {
            _clientRelationshipRepository.Delete(id);
        }
    }
}