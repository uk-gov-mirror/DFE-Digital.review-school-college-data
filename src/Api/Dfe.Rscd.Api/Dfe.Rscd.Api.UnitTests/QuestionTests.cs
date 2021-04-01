﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Amendments;
using Dfe.Rscd.Api.Domain.Entities.Questions;
using Dfe.Rscd.Api.Services;
using Dfe.Rscd.Api.Services.Rules;
using Moq;
using Xunit;

namespace Dfe.Rscd.Api.UnitTests
{
    public class QuestionTests
    {
        [Fact]
        public void ItShouldPresentExpectedLanguageQuestionAndAnswerClassesWithValidators()
        {
            var languageQuestion = new PupilNativeLanguageQuestion(new List<AnswerPotential>());

            Assert.True(languageQuestion.Id != null);
            Assert.True(languageQuestion.Answer.ConditionalQuestion != null);
        }

        [Fact]
        public void ItShouldShowErrorIfAnswerIsNull()
        {
            var dataService = new Mock<IDataService>();
            var establishmentService = new Mock<IEstablishmentService>();
            dataService.Setup(x => x.GetAnswerPotentials(It.IsAny<string>())).Returns(new List<AnswerPotential>());
            var admittedFromAbroadRules = new RemovePupilAdmittedFromAbroadRule(dataService.Object, null, establishmentService.Object);

            var outcome = admittedFromAbroadRules.Apply(new Amendment()
            {
                URN = "100987",
                Answers = new List<UserAnswer>
                {
                    new UserAnswer{Value = string.Empty, QuestionId = "PupilNativeLanguageQuestion"},
                    new UserAnswer{Value = string.Empty, QuestionId = "PupilNativeLanguageQuestion.Other"},
                    new UserAnswer{Value = string.Empty, QuestionId = "PupilCountryQuestion"},
                    new UserAnswer{Value = string.Empty, QuestionId = "PupilCountryQuestion.Other"},
                    new UserAnswer{Value = string.Empty, QuestionId = "ArrivalDateQuestion"},
                    new UserAnswer{Value = string.Empty, QuestionId = "PupilDateOnRollQuestion"}
                }
            });

            Assert.True(outcome.ValidationErrors.Count == 4);
            Assert.True(outcome.IsComplete == false);
        }

        [Fact]
        public void ItShouldShowNotErrorIfDateIsNotEnteredAsNullable()
        {
            var dataService = new Mock<IDataService>();
            var establishmentService = new Mock<IEstablishmentService>();
            dataService.Setup(x => x.GetAnswerPotentials(It.IsAny<string>())).Returns(new List<AnswerPotential>());
            var admittedFromAbroadRules = new RemovePupilAdmittedFromAbroadRule(dataService.Object, null, establishmentService.Object);

            var outcome = admittedFromAbroadRules.Apply(new Amendment()
            {
                URN = "100987",
                Answers = new List<UserAnswer>
                {
                    new UserAnswer{Value = "", QuestionId = "ArrivalDateQuestion"}
                }
            });

            Assert.True(outcome.ValidationErrors == null);
            Assert.True(outcome.IsComplete == false);
        }

        [Fact]
        public void ItShouldShowErrorIfAnswerIsNotEntered()
        {
            var dataService = new Mock<IDataService>();
            var establishmentService = new Mock<IEstablishmentService>();
            dataService.Setup(x => x.GetAnswerPotentials(It.IsAny<string>())).Returns(new List<AnswerPotential>());
            var admittedFromAbroadRules = new RemovePupilAdmittedFromAbroadRule(dataService.Object, null, establishmentService.Object);

            var outcome = admittedFromAbroadRules.Apply(new Amendment()
            {
                URN = "100987",
                Answers = new List<UserAnswer>
                {
                    new UserAnswer{Value = "", QuestionId = nameof(PupilCountryQuestion)}
                }
            });

            Assert.True(outcome.ValidationErrors.Count == 1);
            Assert.True(outcome.ValidationErrors.First().Key == nameof(PupilCountryQuestion));
            Assert.True(outcome.IsComplete == false);
        }

        [Fact]
        public void ItShouldShowErrorIfDateIsNotValid()
        {
            var dataService = new Mock<IDataService>();
            var establishmentService = new Mock<IEstablishmentService>();
            dataService.Setup(x => x.GetAnswerPotentials(It.IsAny<string>())).Returns(new List<AnswerPotential>());
            var admittedFromAbroadRules = new RemovePupilAdmittedFromAbroadRule(dataService.Object, null, establishmentService.Object);

            var outcome = admittedFromAbroadRules.Apply(new Amendment()
            {
                URN = "100987",
                Answers = new List<UserAnswer>
                {
                    new UserAnswer{Value = "23/10/2021", QuestionId = "ArrivalDateQuestion"}
                }
            });

            Assert.True(outcome.ValidationErrors.Count == 1);
            Assert.True(outcome.ValidationErrors.First().Key == nameof(ArrivalDateQuestion));
            Assert.True(outcome.IsComplete == false);
        }

        [Fact]
        public void ItShouldShowErrorIfDateIsNotHistorical()
        {
            var dataService = new Mock<IDataService>();
            var establishmentService = new Mock<IEstablishmentService>();
            dataService.Setup(x => x.GetAnswerPotentials(It.IsAny<string>())).Returns(new List<AnswerPotential>());
            var admittedFromAbroadRules = new RemovePupilAdmittedFromAbroadRule(dataService.Object, null, establishmentService.Object);

            var outcome = admittedFromAbroadRules.Apply(new Amendment()
            {
                URN = "100987",
                Answers = new List<UserAnswer>
                {
                    new UserAnswer{Value = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"), QuestionId = "ArrivalDateQuestion"}
                }
            });

            Assert.True(outcome.ValidationErrors.Count == 1);
            Assert.True(outcome.ValidationErrors.First().Key == nameof(ArrivalDateQuestion));
            Assert.True(outcome.IsComplete == false);
        }

        [Fact]
        public void ItShouldParseDateFormats()
        {
            var dataService = new Mock<IDataService>();
            var establishmentService = new Mock<IEstablishmentService>();
            dataService.Setup(x => x.GetAnswerPotentials(It.IsAny<string>())).Returns(new List<AnswerPotential>());
            var admittedFromAbroadRules = new RemovePupilAdmittedFromAbroadRule(dataService.Object, null, establishmentService.Object);

            var outcome = admittedFromAbroadRules.Apply(new Amendment()
            {
                URN = "100987",
                Answers = new List<UserAnswer>
                {
                    new UserAnswer{Value = "2/1/2021", QuestionId = "ArrivalDateQuestion"}
                }
            });

            Assert.True(outcome.ValidationErrors == null);
            Assert.True(outcome.IsComplete == false);

            outcome = admittedFromAbroadRules.Apply(new Amendment()
            {
                URN = "100987",
                Answers = new List<UserAnswer>
                {
                    new UserAnswer{Value = "2-1-2021", QuestionId = "ArrivalDateQuestion"}
                }
            });

            Assert.True(outcome.ValidationErrors == null);
            Assert.True(outcome.IsComplete == false);

            outcome = admittedFromAbroadRules.Apply(new Amendment()
            {
                URN = "100987",
                Answers = new List<UserAnswer>
                {
                    new UserAnswer{Value = "2-1-21", QuestionId = "ArrivalDateQuestion"}
                }
            });

            Assert.True(outcome.ValidationErrors != null);
            Assert.True(outcome.IsComplete == false);
        }

        [Fact]
        public void ItShouldPresentExpectedCountryQuestionAndAnswerClassesWithValidators()
        {
            var countryQuestion = new PupilCountryQuestion(new List<AnswerPotential>());

            Assert.True(countryQuestion.Id != null);
            Assert.True(countryQuestion.Answer.ConditionalQuestion != null);
        }

        [Fact]
        public void ItShouldPresentExpectedArrivalDateQuestionAndAnswerClassesWithValidators()
        {
            var countryQuestion = new ArrivalDateQuestion();

            Assert.True(countryQuestion.Id != null);
            Assert.True(countryQuestion.Answer.ConditionalQuestion != null);
        }

        [Fact]
        public void ItShouldPresentExpectedQuestionsForAdmittedFromAboard()
        {
            var dataService = new Mock<IDataService>();
            var establishmentService = new Mock<IEstablishmentService>();
            dataService.Setup(x => x.GetAnswerPotentials(It.IsAny<string>())).Returns(new List<AnswerPotential>());
            var admittedFromAbroadRules = new RemovePupilAdmittedFromAbroadRule(dataService.Object, null, establishmentService.Object);

            var outcome = admittedFromAbroadRules.Apply(new Amendment
            {
                URN = "100987"
            });

            Assert.True(outcome.IsComplete == false);
            Assert.True(outcome.FurtherQuestions != null);
            Assert.True(outcome.FurtherQuestions.Count == 3);
        }
    }
}