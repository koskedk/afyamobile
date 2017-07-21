﻿using System;
using LiveHTS.Core.Interfaces.Engine;
using LiveHTS.Core.Interfaces.Repository.Survey;
using LiveHTS.Core.Interfaces.Services.Clients;
using LiveHTS.Core.Model.Interview;
using LiveHTS.Core.Model.Survey;

namespace LiveHTS.Core.Service.Clients
{
    public class ObsService:IObsService
    {
        private Manifest _manifest;
        private readonly Response _response;
        private Encounter _encounter;
        private readonly IDirector _director;
        private readonly IValidator _validator;

        private readonly IFormRepository _formRepository;
        private readonly IEncounterRepository _encounterRepository;
        private readonly IObsRepository _obsRepository;
        


        public Manifest Manifest
        {
            get { return _manifest; }
        }

        public Response Response
        {
            get { return _response; }
        }

        public ObsService(IFormRepository formRepository, IEncounterRepository encounterRepository, IObsRepository obsRepository,Encounter encounter
            , IDirector director,IValidator validator)
        {
            _formRepository = formRepository;
            _encounterRepository = encounterRepository;
            _obsRepository = obsRepository;
            _encounter = encounter;
            _director = director;
            _validator = validator;
        }

        public void Initialize()
        {
            var form = _formRepository.GetWithQuestions(_encounter.FormId, true);
            _manifest = Manifest.Create(form, _encounter);
        }

        public Question GetLiveQuestion()
        {
            return _director.GetLiveQuestion(_manifest);
        }

        public Question GetNextQuestion(Guid currentQuestionId)
        {
            return _manifest.GetNextRankQuestionAfter(currentQuestionId);
        }

        public Question GetPreviousQuestion(Guid currentQuestionId)
        {
            return _manifest.GetPreviousRankQuestionBefore(currentQuestionId);
        }

        public void SaveResponse(Guid encounterId, Guid questionId, object response)
        {
            var liveResponse=new Response(encounterId);

            var question = _manifest.GetQuestion(questionId);
            liveResponse.SetQuestion(question);
            liveResponse.SetObs(encounterId, questionId, question.Concept.ConceptTypeId, response);

            if (_validator.Validate(liveResponse))
            {
                _obsRepository.SaveOrUpdate(liveResponse.Obs);
                UpdateManifest();
            }
        }

        private void UpdateManifest()
        {
            _encounter = _encounterRepository.Load(_encounter.Id, true);
            _manifest.UpdateEncounter(_encounter);
        }
    }
}