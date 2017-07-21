﻿using System;
using LiveHTS.Core.Model.Interview;
using LiveHTS.Core.Model.Survey;

namespace LiveHTS.Core.Interfaces.Services.Clients
{
    public interface IObsService
    {
        Manifest Manifest { get; }
        Response Response { get; }

        void Initialize(Encounter encounter = null);
        
        Question GetLiveQuestion();
        Question GetNextQuestion(Guid currentQuestionId);
        Question GetPreviousQuestion(Guid currentQuestionId);
        Question GetQuestion(Guid questionId, Manifest currentManifest);

        void SaveResponse(Guid encounterId, Guid questionId, object response);
    }
}