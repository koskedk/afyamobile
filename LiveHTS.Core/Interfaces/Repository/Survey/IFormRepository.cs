﻿using System;
using LiveHTS.Core.Model.Survey;

namespace LiveHTS.Core.Interfaces.Repository.Survey
{
    public interface IFormRepository:IRepository<Form,Guid>
    {
        Form GetWithQuestions(Guid formId,bool includeMetadata=false);
    }
}