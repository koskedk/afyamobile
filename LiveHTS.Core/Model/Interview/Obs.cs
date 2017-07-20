﻿using System;
using System.Collections.Generic;
using LiveHTS.SharedKernel.Custom;
using LiveHTS.SharedKernel.Model;
using SQLite;

namespace LiveHTS.Core.Model.Interview
{
    public class Obs:Entity<Guid>
    {
        public Guid QuestionId { get; set; }
        public DateTime ObsDate { get; set; }        
        public string ValueText { get; set; }
        public decimal? ValueNumeric { get; set; }
        public Guid? ValueCoded { get; set; }
        public string ValueMultiCoded { get; set; }
        public DateTime? ValueDateTime { get; set; }
        [Indexed]
        public Guid EncounterId { get; set; }

        public Obs()
        {
            Id = LiveGuid.NewGuid();
            ObsDate=DateTime.Now;
        }

        private Obs(Guid questionId, Guid encounterId):this()
        {
            QuestionId = questionId;
            EncounterId = encounterId;
        }

        private Obs(Guid questionId, Guid encounterId, string value) : this(questionId, encounterId)
        {
            ValueText = value;
        }

        private Obs(Guid questionId, Guid encounterId, Guid? valueCoded):this(questionId,encounterId)
        {
            ValueCoded = valueCoded;
        }
        private Obs(Guid questionId, Guid encounterId, decimal? valueNumeric) : this(questionId, encounterId)
        {
            ValueNumeric = valueNumeric;
        }
       
        private Obs(Guid questionId, Guid encounterId, DateTime? valueDateTime) : this(questionId, encounterId)
        {
            ValueDateTime = valueDateTime;
        }


        public static Obs Create(Guid questionId, Guid encounterId, string type, object obsValue)
        {
            //  Single | Numeric | Multi | DateTime | Text

            var value = obsValue.ToString();

            if (type == "Single")
            {
                var val = string.IsNullOrWhiteSpace(value)?Guid.Empty:new Guid(value);
                return new Obs(questionId, encounterId, val);
            }
            if (type == "Numeric")
            {
                var val = string.IsNullOrWhiteSpace(value) ? -0.01m : Convert.ToDecimal(value);
                return new Obs(questionId, encounterId, val);
            }           
            if (type == "DateTime")
            {
                var val = string.IsNullOrWhiteSpace(value) ? new DateTime(1899,1,1) : Convert.ToDateTime(value);
                return new Obs(questionId, encounterId,val);
            }


            return new Obs(questionId, encounterId, value);
        }


        public bool HasValue(string type)
        {            
            if (type == "Single")
            {
                return ValueCoded.IsNullOrEmpty();
            }
            if (type == "Numeric")
            {
                return null != ValueNumeric && ValueNumeric > -0.01m;
            }
            if (type == "DateTime")
            {
                return null != ValueDateTime && ValueDateTime.Value.Date > new DateTime(1900, 1, 1).Date;
            }
            if (type == "Multi")
            {
                return string.IsNullOrWhiteSpace(ValueMultiCoded);
            }
            if (type == "Text")
            {
                return string.IsNullOrWhiteSpace(ValueText);
            }
            return false;
        }

        public override string ToString()
        {
            return $"{QuestionId} |{ObsDate:T}|";
        }

    }
}