﻿using System;
using System.Collections.Generic;
using LiveHTS.Core.Model.Interview;

namespace LiveHTS.Core.Interfaces.Repository.Survey
{
    public interface IEncounterRepository : IRepository<Encounter, Guid>
    {
        Encounter GetWithObs(Guid encounterId);
        IEnumerable<Encounter> GetWithObs(Guid formId,Guid clientId);
        Encounter GetActiveEncounter(Guid formId, Guid encounterTypeId, Guid clientId, Guid practiceId);
    }
}