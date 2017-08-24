﻿using System;
using System.Collections.Generic;
using LiveHTS.Core.Interfaces;
using LiveHTS.Core.Interfaces.Repository.Interview;
using LiveHTS.Core.Model.Interview;

namespace LiveHTS.Infrastructure.Repository.Interview
{
    public class ObsFinalTestResultRepository : BaseRepository<ObsFinalTestResult, Guid>, IObsFinalTestResultRepository
    {
        public ObsFinalTestResultRepository(ILiveSetting liveSetting) : base(liveSetting)
        {
        }

        public void SaveOrUpdate(ObsFinalTestResult obs)
        {
            var existingObs = Get(obs.Id);
            if (null != existingObs)
            {
                Update(obs);
            }
            else
            {
                Save(obs);
            }
        }

        public List<ObsFinalTestResult> Find(Guid clientId)
        {
            return _db.Query<ObsFinalTestResult>(@"
SELECT 
    ObsFinalTestResult.*
FROM 
    ObsFinalTestResult INNER JOIN Encounter ON ObsFinalTestResult.EncounterId =Encounter.Id
WHERE
    Encounter.ClientId =?", clientId);

        }
    }
}