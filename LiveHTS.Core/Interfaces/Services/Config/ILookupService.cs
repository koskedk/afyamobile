﻿using System.Collections.Generic;
using LiveHTS.Core.Model.Config;

namespace LiveHTS.Core.Interfaces.Services.Config
{
    public interface ILookupService
    {
        IEnumerable<County> GetCounties();
        IEnumerable<SubCounty> GetSubCounties(int[] countyIds);
        IEnumerable<PracticeType> GetPracticeTypes();
        IEnumerable<Practice> GetPractices(string[] typeIds);
    }
}