﻿using System;
using System.Collections.Generic;
using System.Linq;
using LiveHTS.Core.Interfaces.Repository.Interview;
using LiveHTS.Core.Interfaces.Repository.Lookup;
using LiveHTS.Core.Interfaces.Repository.Subject;
using LiveHTS.Core.Interfaces.Services.Interview;
using LiveHTS.Core.Model.Interview;
using LiveHTS.Core.Model.Lookup;
using LiveHTS.Core.Model.Subject;
using LiveHTS.SharedKernel.Custom;
using LiveHTS.SharedKernel.Model;

namespace LiveHTS.Core.Service.Interview
{
    public class MemberScreeningService : IMemberScreeningService
    {
        private readonly IEncounterRepository _encounterRepository;
        private readonly IObsMemberScreeningRepository _obsMemberScreeningRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IClientStateRepository _clientStateRepository;

        private List<CategoryItem> _categoryItems;

        public MemberScreeningService(IEncounterRepository encounterRepository, IObsMemberScreeningRepository obsMemberScreeningRepository, ICategoryRepository categoryRepository, IClientStateRepository clientStateRepository)
        {
            _encounterRepository = encounterRepository;
            _obsMemberScreeningRepository = obsMemberScreeningRepository;
            _categoryRepository = categoryRepository;
            _clientStateRepository = clientStateRepository;
        }

        public Encounter OpenEncounter(Guid encounterId)
        {
            var exisitngEncounter = _encounterRepository.LoadTest(encounterId, true);
            return exisitngEncounter;
        }

        public Encounter StartEncounter(Guid formId, Guid encounterTypeId, Guid clientId, Guid providerId, Guid userId, Guid practiceId, Guid deviceId)
        {
            var exisitngEncounter = _encounterRepository
                .GetAll(x => x.EncounterTypeId == encounterTypeId &&
                             x.ClientId == clientId)
                .FirstOrDefault();

            if (null != exisitngEncounter)
            {
                return OpenEncounter(exisitngEncounter.Id);
            }

            var encounter = Encounter.CreateNew(formId, encounterTypeId, clientId, providerId, userId, practiceId, deviceId);
            encounter.Started = DateTime.Now;
            _encounterRepository.Save(encounter);
            return encounter;
        }

        public IEnumerable<Encounter> LoadEncounter(Guid clientId, Guid encounterTypeId)
        {

            return _encounterRepository.LoadTestAll(encounterTypeId, clientId, true).ToList();
        }

        public void SaveMemberScreening(ObsMemberScreening testResult, Guid clientId)
        {
            _obsMemberScreeningRepository.SaveOrUpdate(testResult);
            _clientStateRepository.SaveOrUpdate(new ClientState(clientId, testResult.EncounterId, LiveState.FamilyScreened));

            _clientStateRepository.DeleteState(clientId, testResult.EncounterId);

            if (testResult.Eligibility== new Guid("b25eccd4-852f-11e7-bb31-be2e44b06b34"))
            {
                _clientStateRepository.SaveOrUpdate(new ClientState(clientId, testResult.EncounterId, LiveState.FamilyEligibileYes));
            }
            else
            {
                _clientStateRepository.SaveOrUpdate(new ClientState(clientId, testResult.EncounterId, LiveState.FamilyEligibileNo));
            }
        }

        public void MarkEncounterCompleted(Guid encounterId, Guid userId, bool completed)
        {
            _encounterRepository.UpdateStatus(encounterId, userId, completed);
        }
    }
}