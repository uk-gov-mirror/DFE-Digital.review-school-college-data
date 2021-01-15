using System.Linq;
using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Dfe.Rscd.Api.UnitTests
{
    public class PupilServicesTests
    {
        public PupilServicesTests()
        {
            Given();
        }

        private void Given()
        {
            _testPupil = Builder.GetPupilDTO("100", "F", "100100", "100200", "100300");

            _repository = new Mock<IDocumentRepository>();
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

        private Mock<IDocumentRepository> _repository;
        private Mock<IConfiguration> _configuration;
        private PupilDTO _testPupil;
        private string _ks4JunePupils;

        [Fact]
        public void WhenGetPupilByIdIsCalledPupilMapsRootFieldsToEntityObject()
        {
            var pupilService = new PupilService(_repository.Object, _configuration.Object);

            var result = pupilService.GetById(CheckingWindow.KS4June, "100");

            _repository.Verify(x => x.GetById<PupilDTO>(_ks4JunePupils, "100"), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.Id == _testPupil.id);
            Assert.True(result.Gender == Gender.Female);
            Assert.True(result.URN == _testPupil.URN);
            Assert.True(result.UPN == _testPupil.UPN);
            Assert.True(result.Age == _testPupil.Age);
            Assert.True(result.DateOfBirth.Year == 2006);
            Assert.True(result.DateOfBirth.Month == 04);
            Assert.True(result.DateOfBirth.Day == 13);
            Assert.True(result.DateOfAdmission.Year == 2020);
            Assert.True(result.DateOfAdmission.Month == 01);
            Assert.True(result.DateOfAdmission.Day == 01);
            Assert.True(result.ForeName == _testPupil.Forename);
            Assert.True(result.LastName == _testPupil.Surname);
            Assert.True(result.AdoptedFromCareId == _testPupil.AdoptedFromCareID);
            Assert.True(result.Ethnicity == _testPupil.EthnicityCode);
            Assert.True(result.FreeSchoolMeals == true);
            Assert.True(result.FirstLanguage == _testPupil.FirstLanguageCode);
            Assert.True(result.ForvusNumber == int.Parse(_testPupil.ForvusIndex));
            Assert.True(result.DfesNumber == _testPupil.DFESNumber);
            Assert.True(result.PIncludeId == _testPupil.P_INCL);
            Assert.True(result.SenStatus == _testPupil.SENStatusCode);
        }

        [Fact]
        public void WhenSearchPupilByIdShouldReturnFoundPupil1000ByUPN()
        {
            var pupilService = new PupilService(_repository.Object, _configuration.Object);

            var result = pupilService.QueryPupils(CheckingWindow.KS4June, new PupilsSearchRequest {ID = "11"});

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.Count == 1);
            Assert.True(result.First().Id == "1000");
        }

        [Fact]
        public void WhenSearchPupilByIdShouldReturnFoundPupil1000ByULN()
        {
            var pupilService = new PupilService(_repository.Object, _configuration.Object);

            var result = pupilService.QueryPupils(CheckingWindow.KS4June, new PupilsSearchRequest {ID = "101"});

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.Count == 1);
            Assert.True(result.First().Id == "1000");
        }

        [Fact]
        public void WhenSearchPupilByIdShouldReturnFoundPupil2000ByUPN()
        {
            var pupilService = new PupilService(_repository.Object, _configuration.Object);

            var result = pupilService.QueryPupils(CheckingWindow.KS4June, new PupilsSearchRequest {ID = "22"});

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.Count == 1);
            Assert.True(result.First().Id == "2000");
        }

        [Fact]
        public void WhenSearchPupilByIdShouldReturnFoundPupil2000ByULN()
        {
            var pupilService = new PupilService(_repository.Object, _configuration.Object);

            var result = pupilService.QueryPupils(CheckingWindow.KS4June, new PupilsSearchRequest {ID = "202"});

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.Count == 1);
            Assert.True(result.First().Id == "2000");
        }
        [Fact]
        public void WhenSearchPupilByNameShouldReturnMatchedPupils()
        {
            var pupilService = new PupilService(_repository.Object, _configuration.Object);

            var result = pupilService.QueryPupils(CheckingWindow.KS4June, new PupilsSearchRequest {Name = "Mar"});

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.Count == 2);
            Assert.True(result.First().Id == "3000");
            Assert.True(result.Last().Id == "4000");
        }

        [Fact]
        public void WhenSearchPupilByURNShouldReturnMatchedPupilForU111ByURN()
        {
            var pupilService = new PupilService(_repository.Object, _configuration.Object);

            var result = pupilService.QueryPupils(CheckingWindow.KS4June, new PupilsSearchRequest {URN = "U111"});

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.Count == 1);
            Assert.True(result.First().Id == "1000");
        }

        [Fact]
        public void WhenSearchPupilByURNShouldReturnMatchedPupilForU222ByURN()
        {
            var pupilService = new PupilService(_repository.Object, _configuration.Object);

            var result = pupilService.QueryPupils(CheckingWindow.KS4June, new PupilsSearchRequest {URN = "U222"});

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.Count == 1);
            Assert.True(result.First().Id == "2000");
        }

        [Fact]
        public void WhenGetPupilByIdIsCalledPupilMapsAllocationYearsToEntityObject()
        {
            var pupilService = new PupilService(_repository.Object, _configuration.Object);

            var result = pupilService.GetById(CheckingWindow.KS4June, "100");

            _repository.Verify(x => x.GetById<PupilDTO>(_ks4JunePupils, "100"), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.Allocations.Count == 3);
            Assert.True(result.Allocations.First().Allocation == Allocation.AwardingOrganisation);
            Assert.True(result.Allocations.First().Year == 2021);
            Assert.True(result.Allocations.Skip(1).First().Allocation == Allocation.NotAllocated);
            Assert.True(result.Allocations.Skip(1).First().Year == 2020);
            Assert.True(result.Allocations.Last().Allocation == Allocation.NotAllocated);
            Assert.True(result.Allocations.Last().Year == 2019);
        }

        [Fact]
        public void WhenGetPupilByIdIsCalledPupilMapsPerformanceToEntityObject()
        {
            var pupilService = new PupilService(_repository.Object, _configuration.Object);

            var result = pupilService.GetById(CheckingWindow.KS4June, "100");

            _repository.Verify(x => x.GetById<PupilDTO>(_ks4JunePupils, "100"), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.Results.Count == 1);
            Assert.True(result.Results.First().SubjectCode == _testPupil.performance.First().SubjectCode);
            Assert.True(result.Results.First().ExamYear == _testPupil.performance.First().ExamYear);
            Assert.True(result.Results.First().TestMark== _testPupil.performance.First().TestMark);
            Assert.True(result.Results.First().ScaledScore == _testPupil.performance.First().ScaledScore);
        }
    }
}