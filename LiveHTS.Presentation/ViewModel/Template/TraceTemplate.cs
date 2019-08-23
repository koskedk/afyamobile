﻿using System;
using System.Collections.Generic;
using System.Linq;
using LiveHTS.Core.Model.Interview;
using LiveHTS.Core.Model.Lookup;
using LiveHTS.Presentation.Interfaces.ViewModel.Template;
using LiveHTS.Presentation.Interfaces.ViewModel.Wrapper;
using LiveHTS.Presentation.Validations;
using LiveHTS.SharedKernel.Custom;
using MvvmCross.Core.ViewModels;
using MvvmValidation;

namespace LiveHTS.Presentation.ViewModel.Template
{
    public class TraceTemplate:MvxNotifyPropertyChanged, ITraceTemplate
    {

        public ObsTraceResult TraceResult { get; set; }
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Guid Mode { get; set; }
        public string ModeDisplay { get; set; }
        public Guid Outcome { get; set; }
        public string OutcomeDisplay { get; set; }
        public Guid ReasonNotContacted { get; set; }

        public string ReasonNotContactedDisplay { get; set; }
        public string ReasonNotContactedOther { get; set; }
        public Guid EncounterId { get; set; }

        public TraceTemplate(ObsTraceResult testResult, List<CategoryItem> modes, List<CategoryItem> outcomes,List<CategoryItem> reasons)
        {
            TraceResult = testResult;

            if (null != modes && modes.Count > 0)
            {
                var kit = modes.FirstOrDefault(x => x.ItemId == testResult.Mode);
                if (null != kit)
                {
                    ModeDisplay = kit.Display;
                }
            }

            if (null != outcomes && outcomes.Count > 0)
            {
                var result = outcomes.FirstOrDefault(x => x.ItemId == testResult.Outcome);
                if (null != result)
                {
                    OutcomeDisplay = result.Display;

                }
            }

            if (null != reasons && reasons.Count > 0)
            {
                var result = reasons.FirstOrDefault(x =>
                    !testResult.ReasonNotContacted.IsNullOrEmpty() && x.ItemId == testResult.ReasonNotContacted);
                if (null != result)
                {
                    ReasonNotContactedDisplay = result.Display;

                }
            }

            Id = testResult.Id;
            Date = testResult.Date;
            Mode = testResult.Mode;
            Outcome = testResult.Outcome;
            if(!testResult.ReasonNotContacted.IsNullOrEmpty())
                ReasonNotContacted = testResult.ReasonNotContacted.Value;
            ReasonNotContactedOther = testResult.ReasonNotContactedOther;
            EncounterId = testResult.EncounterId;
        }


    }
}
