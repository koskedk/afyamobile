﻿using System;
using System.Collections.Generic;
using System.Linq;
using LiveHTS.Core.Model.Survey;
using LiveHTS.SharedKernel.Custom;
using SQLite;

namespace LiveHTS.Core.Model.Interview
{
    public class Manifest
    {
        public Encounter Encounter { get; set; }
        public List<Question> QuestionStore { get; set; } = new List<Question>();
        public List<Response> ResponseStore { get; set; } = new List<Response>();

        public bool HasQuestions()
        {
            return null != QuestionStore && QuestionStore.Count > 0;
        }

        public bool HasResponses()
        {
            return null != ResponseStore && ResponseStore.Count > 0;
        }
       

        public Manifest()
        {
        }

        private Manifest(List<Question> questions, List<Response> responses, Encounter encounter)
        {
            QuestionStore = questions;
            ResponseStore = responses;
            Encounter = encounter;
            UpdateResponseQuestions();
        }

        public static Manifest Create(Form form, Encounter encounter)
        {
            var formWithQuestions = form ?? new Form();
            var responses = ReadResponses(encounter);

            return new Manifest(formWithQuestions.Questions, responses, encounter);
        }

        public void UpdateEncounter(Encounter encounter)
        {
            Encounter = encounter;
            ResponseStore = ReadResponses(Encounter);
            UpdateResponseQuestions();
        }

        private void UpdateResponseQuestions()
        {
            foreach (var response in ResponseStore)
            {
                response.Question = GetQuestion(response.QuestionId);
            }
        }

        private static List<Response> ReadResponses(Encounter encounter)
        {
            var answeredQuestions = new List<Response>();

            if (null != encounter)
            {
                answeredQuestions = encounter.Obses.Select(x => new Response
                    {
                        EncounterId = x.EncounterId,
                        ObsId = x.Id,
                        Obs = x,
                        QuestionId = x.QuestionId
                    })
                    .ToList();
            }

            return answeredQuestions;
        }

        public Question GetFirstQuestion()
        {
            return QuestionStore.OrderBy(x => x.Rank).FirstOrDefault();
        }

        public Response GetLastResponse()
        {
            return HasResponses() ? ResponseStore.OrderBy(x => x.Question.Rank).LastOrDefault() : null;
        }

        public override string ToString()
        {
            var stats = $"{ResponseStore.Count}/{QuestionStore.Count}";
            var summary = Encounter.IsComplete ? " Completed" : " Open";
            return $" Status:{stats} ,{summary}";
        }

        public Question GetQuestion(Guid value)

        {
            return QuestionStore.FirstOrDefault(x => x.Id == value);
        }

        public Question GetNextRankQuestionAfter(Guid currentQuestionId)
        {
            var currenQuestion = GetQuestion(currentQuestionId);
            var currentRank = currenQuestion.Rank;
            var currentId = currenQuestion.Id;

            return QuestionStore
                .OrderBy(x => x.Rank)
                .FirstOrDefault(x => x.Rank >= currentRank &&
                                     x.Id != currentId);
        }

        public Question GetPreviousRankQuestionBefore(Guid currentQuestionId)
        {
            var currenQuestion = GetQuestion(currentQuestionId);
            var currentRank = currenQuestion.Rank;
            var currentId = currenQuestion.Id;

            return QuestionStore
                .OrderBy(x=>x.Rank)
                .FirstOrDefault(x => x.Rank <= currentRank &&
                                     x.Id != currentId);
        }
    }
}