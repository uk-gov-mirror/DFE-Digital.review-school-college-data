using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Dfe.Rscd.Api.UnitTests
{
    [TestFixture]
    public class PupilServicesTests
    {
        private Mock<IRepository> _repository;
        private Mock<IConfiguration> _configuration;
        private PupilDTO _testPupil;
        private string _ks4JunePupils;

        [SetUp]
        public void Given()
        {
            _testPupil = Builder.GetPupilDTO("100", "F", "100100", "100200", "100300");

            _repository = new Mock<IRepository>();
            _ks4JunePupils = "ks4june_pupils_2021";

            _repository.Setup(x =>
                    x.GetById<PupilDTO>(_ks4JunePupils,
                        It.IsAny<string>()))
                .Returns(_testPupil);

            _repository.Setup(x =>
                    x.Get<PupilDTO>(_ks4JunePupils))
                .Returns(Builder.GetPupils().AsQueryable());

            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(x => x["AllocationYear"]).Returns("2021");
        }

        [Test]
        public void WhenGetPupilByIdIsCalledPupilMapsRootFieldsToEntityObject()
        {
            var pupilService = new PupilService(_repository.Object, _configuration.Object);

            var result = pupilService.GetById(CheckingWindow.KS4June, "100");

            _repository.Verify(x=>x.GetById<PupilDTO>(_ks4JunePupils, "100"), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(_testPupil.id));
            Assert.That(result.Gender, Is.EqualTo(Gender.Female));
            Assert.That(result.URN, Is.EqualTo(_testPupil.URN));
            Assert.That(result.UPN, Is.EqualTo(_testPupil.UPN));
            Assert.That(result.Age, Is.EqualTo(_testPupil.Age));
            Assert.That(result.DateOfBirth.Year, Is.EqualTo(2006));
            Assert.That(result.DateOfBirth.Month, Is.EqualTo(04));
            Assert.That(result.DateOfBirth.Day, Is.EqualTo(13));
            Assert.That(result.DateOfAdmission.Year, Is.EqualTo(2020));
            Assert.That(result.DateOfAdmission.Month, Is.EqualTo(01));
            Assert.That(result.DateOfAdmission.Day, Is.EqualTo(01));
            Assert.That(result.ForeName, Is.EqualTo(_testPupil.Forename));
            Assert.That(result.LastName, Is.EqualTo(_testPupil.Surname));
            Assert.That(result.AdoptedFromCareId, Is.EqualTo(_testPupil.AdoptedFromCareID));
            Assert.That(result.Ethnicity, Is.EqualTo(_testPupil.EthnicityCode));
            Assert.That(result.FreeSchoolMeals, Is.EqualTo(true));
            Assert.That(result.FirstLanguage, Is.EqualTo(_testPupil.FirstLanguageCode));
            Assert.That(result.ForvusNumber, Is.EqualTo(int.Parse(_testPupil.ForvusIndex)));
            Assert.That(result.DfesNumber, Is.EqualTo(_testPupil.DFESNumber));
            Assert.That(result.PIncludeId, Is.EqualTo(_testPupil.P_INCL));
            Assert.That(result.SenStatus, Is.EqualTo(_testPupil.SENStatusCode));
        }

        [Test]
        public void WhenSearchPupilByIdShouldReturnFoundPupil1000ByUPN()
        {
            var pupilService = new PupilService(_repository.Object, _configuration.Object);

            var result = pupilService.QueryPupils(CheckingWindow.KS4June, new PupilsSearchRequest { ID = "11"});

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo("1000"));
        }

        [Test]
        public void WhenSearchPupilByIdShouldReturnFoundPupil1000ByULN()
        {
            var pupilService = new PupilService(_repository.Object, _configuration.Object);

            var result = pupilService.QueryPupils(CheckingWindow.KS4June, new PupilsSearchRequest { ID = "101" });

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo("1000"));
        }

        [Test]
        public void WhenSearchPupilByIdShouldReturnFoundPupil2000ByUPN()
        {
            var pupilService = new PupilService(_repository.Object, _configuration.Object);

            var result = pupilService.QueryPupils(CheckingWindow.KS4June, new PupilsSearchRequest { ID = "22" });

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo("2000"));
        }

        [Test]
        public void WhenSearchPupilByIdShouldReturnFoundPupil2000ByULN()
        {
            var pupilService = new PupilService(_repository.Object, _configuration.Object);

            var result = pupilService.QueryPupils(CheckingWindow.KS4June, new PupilsSearchRequest { ID = "202" });

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo("2000"));
        }

        [Test]
        public void WhenSearchPupilByNameShouldReturnMatchedPupils()
        {
            var pupilService = new PupilService(_repository.Object, _configuration.Object);

            var result = pupilService.QueryPupils(CheckingWindow.KS4June, new PupilsSearchRequest { Name = "Mar" });

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result.First().Id, Is.EqualTo("3000"));
            Assert.That(result.Last().Id, Is.EqualTo("4000"));
        }

        [Test]
        public void WhenSearchPupilByURNShouldReturnMatchedPupilForU111ByURN()
        {
            var pupilService = new PupilService(_repository.Object, _configuration.Object);

            var result = pupilService.QueryPupils(CheckingWindow.KS4June, new PupilsSearchRequest { URN = "U111" });

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo("1000"));
        }

        [Test]
        public void WhenSearchPupilByURNShouldReturnMatchedPupilForU222ByURN()
        {
            var pupilService = new PupilService(_repository.Object, _configuration.Object);

            var result = pupilService.QueryPupils(CheckingWindow.KS4June, new PupilsSearchRequest { URN = "U222" });

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo("2000"));
        }

        [Test]
        public void WhenGetPupilByIdIsCalledPupilMapsAllocationYearsToEntityObject()
        {
            var pupilService = new PupilService(_repository.Object, _configuration.Object);

            var result = pupilService.GetById(CheckingWindow.KS4June, "100");

            _repository.Verify(x => x.GetById<PupilDTO>(_ks4JunePupils, "100"), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Allocations, Has.Count.EqualTo(3));
            Assert.That(result.Allocations.First().Allocation, Is.EqualTo(Allocation.AwardingOrganisation));
            Assert.That(result.Allocations.First().Year, Is.EqualTo(2021));
            Assert.That(result.Allocations.Skip(1).First().Allocation, Is.EqualTo(Allocation.NotAllocated));
            Assert.That(result.Allocations.Skip(1).First().Year, Is.EqualTo(2020));
            Assert.That(result.Allocations.Last().Allocation, Is.EqualTo(Allocation.NotAllocated));
            Assert.That(result.Allocations.Last().Year, Is.EqualTo(2019));
        }

        [Test]
        public void WhenGetPupilByIdIsCalledPupilMapsPerformanceToEntityObject()
        {
            var pupilService = new PupilService(_repository.Object, _configuration.Object);

            var result = pupilService.GetById(CheckingWindow.KS4June, "100");

            _repository.Verify(x => x.GetById<PupilDTO>(_ks4JunePupils, "100"), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Results.Count, Is.EqualTo(1));
            Assert.That(result.Results.First().SubjectCode, Is.EqualTo(_testPupil.performance.First().SubjectCode));
            Assert.That(result.Results.First().ExamYear, Is.EqualTo(_testPupil.performance.First().ExamYear));
            Assert.That(result.Results.First().TestMark, Is.EqualTo(_testPupil.performance.First().TestMark));
            Assert.That(result.Results.First().ScaledScore, Is.EqualTo(_testPupil.performance.First().ScaledScore));
        }
    }
}
