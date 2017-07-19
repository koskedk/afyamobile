﻿using System;
using System.Linq;
using LiveHTS.Core.Interfaces;
using LiveHTS.Core.Model.Interview;
using LiveHTS.Core.Service;
using LiveHTS.Infrastructure.Repository.Survey;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLite;

namespace LiveHTS.Core.Tests.Service
{
    [TestClass]
    public class DirectorServiceTests
    {
        private DirectorService _directorService;
        private ILiveSetting _liveSetting;
        private SQLiteConnection _database = TestHelpers.GetDatabase();
        private Guid _formId, _encounterTypeId, _clientId, _practiceId, _deviceId, _providerId, _userId;
        private Encounter _encounter;

         [TestInitialize]
        public void SetUp()
        {
            _liveSetting = new LiveSetting(_database.DatabasePath);
            _directorService = new DirectorService(
                new FormRepository(_liveSetting,
                    new QuestionRepository(_liveSetting,
                        new ConceptRepository(_liveSetting, new CategoryRepository(_liveSetting)))),
                new EncounterRepository(_liveSetting));
            _formId = TestDataHelpers._formId;
            _encounterTypeId = TestDataHelpers._encounterTypeId;
            _clientId = TestDataHelpers._clients.First().Id;
            _practiceId = TestDataHelpers._formId;
            _encounter = TestDataHelpers.Encounters.First();
        }

        [TestMethod]
        public void should_Refresh_Manifest()
        {
            _directorService.RefreshManifest(_formId,_encounterTypeId,_clientId,_practiceId);
            var manifest=_directorService.Manifest;
            Assert.IsNotNull(manifest);
            Console.WriteLine(manifest);
        }


        [TestMethod]
        public void should_Start_Encounter_New()
        {
            _directorService.RefreshManifest(_formId, _encounterTypeId, _clientId,_practiceId);
            var encounter = _directorService.StartEncounter(_practiceId, _deviceId, _providerId, _userId);
            var manifest = _directorService.Manifest;
            Assert.IsNotNull(encounter);
            Assert.IsNotNull(manifest);
            Console.WriteLine($"NEW:{encounter}|{manifest}");
        }

        [TestMethod]
        public void should_Start_Encounter_Existing()
        {
            
            _directorService.RefreshManifest(_encounter.FormId,_encounter.EncounterTypeId, _encounter.ClientId,_encounter.PracticeId);
            var encounter = _directorService.StartEncounter(_encounter.PracticeId, _encounter.DeviceId, _encounter.ProviderId, _encounter.UserId);
            var manifest = _directorService.Manifest;
            Assert.IsNotNull(encounter);
            Assert.IsNotNull(manifest);
            Console.WriteLine($"OPEN:{encounter}|{manifest}");
        }
    }
}