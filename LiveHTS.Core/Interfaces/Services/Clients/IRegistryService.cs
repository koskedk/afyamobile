﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveHTS.Core.Model.Config;
using LiveHTS.Core.Model.Interview;
using LiveHTS.Core.Model.SmartCard;
using LiveHTS.Core.Model.Subject;

namespace LiveHTS.Core.Interfaces.Services.Clients
{
    public interface IRegistryService
    {
        Client Load(Guid id);
        Client Find(Guid id);
        IEnumerable<Client> GetAllClients(string search="");
        IEnumerable<Client> GetAllSiteClients(Guid siteId, string search = "");
        IEnumerable<Client> GetAllCohortClients(Guid cohortId,string search = "");
        void Save(Client client);
        void Save(Client client,Guid cohortId);
        void UpdateRelationShips(string relationshipTypeId,Guid clientId, Guid otherClientId);
        void SaveOrUpdate(Client client,bool isClient=true);
        Guid SaveOrGet(Client client, bool isClient = true, bool isTested = true);
        void SaveDownloaded(Client client);
        Task Download(Client client,List<Encounter> encounters);
        Task<Guid> SaveShr(Client shrClient,bool isTested=true);
        void UpdateSmartCardEnrolled(Guid clientId);
        void UpdateSmartCardShr(Guid clientId, string shr);
        void Delete(Guid clientId);
    }
}