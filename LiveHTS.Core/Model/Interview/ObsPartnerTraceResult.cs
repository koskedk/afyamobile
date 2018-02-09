﻿using System;
using LiveHTS.SharedKernel.Custom;
using LiveHTS.SharedKernel.Model;
using SQLite;

namespace LiveHTS.Core.Model.Interview
{
    public class ObsPartnerTraceResult : Entity<Guid>
    {
        public DateTime Date { get; set; }
        [Indexed]
        public Guid Mode { get; set; }
        [Ignore]
        public string ModeDisplay { get; set; }
        [Indexed]
        public Guid Outcome { get; set; }
        [Ignore]
        public string OutcomeDisplay { get; set; }
        public Guid? Consent { get; set; }
        public DateTime BookingDate { get; set; }
        public Guid EncounterId { get; set; }

        public ObsPartnerTraceResult()
        {
            Id = LiveGuid.NewGuid();
        }

        public ObsPartnerTraceResult(DateTime date, Guid mode, Guid outcome,Guid? consent, DateTime bookingDate ,Guid encounterId) : this()
        {
            Date = date;
            Mode = mode;
            Outcome = outcome;
            Consent = consent;
            BookingDate = bookingDate;
            EncounterId = encounterId;
        }

        public static ObsPartnerTraceResult Create(Guid id, DateTime date, Guid mode, Guid outcome,Guid? consent, DateTime bookingDate, Guid encounterId)
        {
            var obs = new ObsPartnerTraceResult(date, mode, outcome, consent, bookingDate,encounterId);
            obs.Id = id;
            return obs;
        }
        public static ObsPartnerTraceResult Create(DateTime date, Guid mode, Guid outcome, Guid? consent, DateTime bookingDate, Guid encounterId)
        {
            return new ObsPartnerTraceResult(date, mode, outcome, consent, bookingDate, encounterId);
        }
        public static ObsPartnerTraceResult CreateNew(DateTime date, Guid mode, Guid outcome,Guid? consent, DateTime bookingDate, Guid encounterId)
        {
            return new ObsPartnerTraceResult(date, mode, outcome, consent, bookingDate, encounterId);
        }
        public static ObsPartnerTraceResult CreateNew(Guid encounterId)
        {
            var obs = new ObsPartnerTraceResult();
            obs.Date = DateTime.Today;
            obs.EncounterId = encounterId;
            return obs;
        }


    }
}