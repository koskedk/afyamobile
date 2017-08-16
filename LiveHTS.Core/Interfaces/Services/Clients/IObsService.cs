﻿using System;
using LiveHTS.Core.Model.Interview;
using LiveHTS.Core.Model.Survey;

namespace LiveHTS.Core.Interfaces.Services.Clients
{
    public interface IObsService
    {
        Manifest Manifest { get; }
        Response Response { get; }

        void Initialize(Encounter encounter);
        
        Question GetLiveQuestion(Manifest manifest=null);        
        Question GetNextQuestion(Guid currentQuestionId, Manifest manifest = null);
        Question GetPreviousQuestion(Guid currentQuestionId);
        Question GetQuestion(Guid questionId, Manifest currentManifest);

        bool ValidateResponse(Guid encounterId, Guid questionId, object response);
        void SaveResponse(Guid encounterId, Guid questionId, object response);
    }
}