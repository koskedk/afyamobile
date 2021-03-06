﻿using System;
using System.Linq;
using LiveHTS.Core;
using LiveHTS.Core.Interfaces;
using LiveHTS.Core.Interfaces.Repository.Survey;
using LiveHTS.Infrastructure.Migrations;
using LiveHTS.Infrastructure.Repository.Survey;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLite;


namespace LiveHTS.Infrastructure.Tests.Repository.Survey
{
    [TestClass]
    public class QuestionRepositoryTests
    {
        private IQuestionRepository _questionRepository;
        private SQLiteConnection _connection;


        [TestInitialize]
        public void SetUp()
        {
            _connection = new SQLiteConnection("testlivehts.db");
            Seeder.Seed(_connection);
            _questionRepository = new QuestionRepository(new LiveSetting(_connection.DatabasePath), 
                new ConceptRepository(new LiveSetting(_connection.DatabasePath), new CategoryRepository(new LiveSetting(_connection.DatabasePath))));
        }

        [TestMethod]
        public void should_Get_Question_with_Concept()
        {
            var questions = _questionRepository.GetWithConcepts().ToList();
            Assert.IsTrue(questions.Count > 0);
            foreach (var question in questions)
            {
                Assert.IsNotNull(question);
                Console.Write(question);
                Assert.IsNotNull(question.Concept);
                Console.Write($" [{question.Concept}]");
                if (question.Concept.CategoryId.HasValue)
                {
                    Assert.IsNotNull(question.Concept.Category);
                    Assert.AreEqual(question.Concept.CategoryId, question.Concept.Category.Id);
                    Console.Write($" ({question.Concept.Category})");
                    Console.WriteLine();
                    foreach (var categoryItem in question.Concept.Category.Items)
                    {
                        Console.WriteLine($"   >. {categoryItem}");
                    }
                }
                else
                {
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        [TestMethod]
        public void should_Get_Question_with_Metadata_Branches()
        {
            var questions = _questionRepository.GetWithMetadata().ToList();
            Assert.IsTrue(questions.Count > 0);
            var questionMetadata = questions.Where(x => x.HasBranches).ToList();
            Assert.IsTrue(questionMetadata.Count>0);
            foreach (var question in questionMetadata)
            {
                Console.Write(question);
                foreach (var branch in question.Branches)
                {
                    Console.WriteLine(branch);
                }
            }

        }
        [TestMethod]
        public void should_Get_Question_with_Metadata_Validations()
        {
            var questions = _questionRepository.GetWithMetadata().ToList();
            Assert.IsTrue(questions.Count > 0);
            var questionMetadata = questions.Where(x => x.HasValidations).ToList();
            Assert.IsTrue(questionMetadata.Count > 0);
            foreach (var question in questionMetadata)
            {
                Console.WriteLine(question);
                foreach (var validation in question.Validations)
                {
                    Console.WriteLine($" >. {validation}");
                }
            }

        }
        [TestMethod]
        public void should_Get_Question_with_Metadata_ReValidations()
        {
            var questions = _questionRepository.GetWithMetadata().ToList();
            Assert.IsTrue(questions.Count > 0);
            var questionMetadata = questions.Where(x => x.HasReValidations).ToList();
            Assert.IsTrue(questionMetadata.Count > 0);
            foreach (var question in questionMetadata)
            {
                Console.WriteLine(question);
                foreach (var validation in question.ReValidations)
                {
                    Console.WriteLine($" >. {validation}");
                }
            }

        }
        [TestMethod]
        public void should_Get_Question_with_Metadata_Transformation()
        {
            var questions = _questionRepository.GetWithMetadata().ToList();
            Assert.IsTrue(questions.Count > 0);
            var questionMetadata = questions.Where(x => x.HasTransformations).ToList();
            Assert.IsTrue(questionMetadata.Count > 0);
            foreach (var question in questionMetadata)
            {
                Console.WriteLine(question);
                foreach (var validation in question.Transformations)
                {
                    Console.WriteLine($" >. {validation}");
                }
            }

        }
        [TestMethod]
        public void should_Get_Question_with_Metadata_RemoteTransformation()
        {
            var questions = _questionRepository.GetWithMetadata().ToList();
            Assert.IsTrue(questions.Count > 0);
            var questionMetadata = questions.Where(x => x.HasRemoteTransformations).ToList();
            Assert.IsTrue(questionMetadata.Count > 0);
            foreach (var question in questionMetadata)
            {
                Console.WriteLine(question);
                foreach (var validation in question.RemoteTransformations)
                {
                    Console.WriteLine($" >. {validation}");
                }
            }

        }
    }
}