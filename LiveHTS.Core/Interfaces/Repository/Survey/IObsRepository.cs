﻿using System;
using System.Collections.Generic;
using LiveHTS.Core.Model.Interview;

namespace LiveHTS.Core.Interfaces.Repository.Survey
{
    public interface IObsRepository : IRepository<Obs, Guid>
    {
        void SaveOrUpdate(Obs obs);
    }
}