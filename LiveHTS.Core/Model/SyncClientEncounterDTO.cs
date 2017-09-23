﻿using System;
using System.Collections.Generic;
using System.Linq;
using LiveHTS.Core.Model.Interview;
using LiveHTS.Core.Model.Subject;

namespace LiveHTS.Core.Model
{
    public class SyncClientEncounterDTO
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Guid FormId { get; set; }
        public Guid EncounterTypeId { get; set; }
        public DateTime EncounterDate { get; set; }
        public Guid ProviderId { get; set; }
        public Guid DeviceId { get; set; }
        public Guid PracticeId { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Stopped { get; set; }
        public string KeyPop { get; set; }
        public string OtherKeyPop { get; set; }
        public string Phone { get; set; }
        public IEnumerable<Obs> Obses { get; set; } = new List<Obs>();
        public IEnumerable<ObsTestResult> ObsTestResults { get; set; } = new List<ObsTestResult>();
        public IEnumerable<ObsFinalTestResult> ObsFinalTestResults { get; set; } = new List<ObsFinalTestResult>();
        public IEnumerable<ObsTraceResult> ObsTraceResults { get; set; } = new List<ObsTraceResult>();
        public IEnumerable<ObsLinkage> ObsLinkages { get; set; } = new List<ObsLinkage>();
        public Guid UserId { get; set; }
        public bool IsComplete { get; set; }

        public SyncClientEncounterDTO(Encounter encounter, Client client)
        {
            Id = encounter.Id;
            ClientId = encounter.ClientId;
            FormId = encounter.FormId;
            EncounterTypeId = encounter.EncounterTypeId;
            EncounterDate = encounter.EncounterDate;
            ProviderId = encounter.ProviderId;
            DeviceId = encounter.DeviceId;
            PracticeId = encounter.PracticeId;
            Started = encounter.Started;
            Stopped = encounter.Stopped;
            Obses = encounter.Obses.ToList();
            ObsTestResults = encounter.ObsTestResults.ToList();
            ObsFinalTestResults = encounter.ObsFinalTestResults.ToList();
            ObsTraceResults = encounter.ObsTraceResults.ToList();
            ObsLinkages = encounter.ObsLinkages.ToList();
            UserId = encounter.UserId;
            IsComplete = encounter.IsComplete;
            KeyPop = client.KeyPop;
            OtherKeyPop = client.OtherKeyPop;

            try
            {
                Phone = client.Person.Contacts.FirstOrDefault().Phone.ToString();
            }
            catch 
            {
            }
            

        }

        public static List<SyncClientEncounterDTO> Create(List<Encounter> encounters, Client client)
        {
            var list=new List<SyncClientEncounterDTO>();
            foreach (var e in encounters)
            {
                list.Add(new SyncClientEncounterDTO(e,client));
            }
            return list;
        }
    }
}