using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories;
using Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs;
using Dfe.Rscd.Api.Infrastructure.SqlServer.Repositories;
using Dfe.Rscd.Api.Services;
using Moq;
using Xunit;
using Ethnicity = Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs.Ethnicity;

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
            _dataRepository = new Mock<IDataRepository>();

            _ks4JunePupils = "ks4june_pupils_2021";
            _ks4JuneEstablishments = "ks4june_establishments_2021";

            _repository.Setup(x =>
                    x.Get<EstablishmentDTO>(_ks4JuneEstablishments))
                .Returns(Builder.GetEstablishment(123232).AsQueryable());

            _dataRepository.Setup(x =>
                    x.Get<Pincl>())
                .Returns(Builder.GetPincLsList().AsQueryable());

            _dataRepository.Setup(x =>
                    x.Get<Ethnicity>())
                .Returns(Builder.GetEthnicityList().AsQueryable());

            _dataRepository.Setup(x =>
                    x.Get<Senstatus>())
                .Returns(Builder.GetSenList().AsQueryable());

            _dataRepository.Setup(x =>
                    x.Get<Language>())
                .Returns(Builder.GetLanguageList().AsQueryable());

            _configuration = new Mock<IAllocationYearConfig>();
            _configuration.Setup(x => x.Value).Returns("2021");

            _pupilService = new PupilService(_repository.Object, new DataService(_dataRepository.Object),
                new EstablishmentService(_repository.Object, _configuration.Object), _configuration.Object);
        }

        private void GivenRepoReturnsSingleTestPupil()
        {
            _repository.Setup(x =>
                    x.Get<PupilDTO>(_ks4JunePupils))
                .Returns(new List<PupilDTO> { _testPupil }.AsQueryable());
        }

        private void GivenRepoReturnsMultipleTestPupils()
        {
            _repository.Setup(x =>
                    x.Get<PupilDTO>(_ks4JunePupils))
                .Returns(Builder.GetPupils().AsQueryable());
        }

        private Mock<IDocumentRepository> _repository;
        private Mock<IAllocationYearConfig> _configuration;
        private Mock<IDataRepository> _dataRepository;

        private PupilService _pupilService;

        private PupilDTO _testPupil;
        private string _ks4JunePupils;
        private string _ks4JuneEstablishments;

        [Fact]
        public void WhenGetPupilByIdIsCalledPupilMapsRootFieldsToEntityObject()
        {
            GivenRepoReturnsSingleTestPupil();

            var result = _pupilService.GetById(CheckingWindow.KS4June, "100");

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.Id == _testPupil.id);
            Assert.True(result.Gender.Code == "F");
            Assert.True(result.URN == _testPupil.URN);
            Assert.True(result.UPN == _testPupil.UPN);
            Assert.True(result.Age == _testPupil.Age);
            Assert.True(result.DOB.Year == 2006);
            Assert.True(result.DOB.Month == 04);
            Assert.True(result.DOB.Day == 13);
            Assert.True(result.AdmissionDate.Year == 2020);
            Assert.True(result.AdmissionDate.Month == 01);
            Assert.True(result.AdmissionDate.Day == 01);
            Assert.True(result.Forename == _testPupil.Forename);
            Assert.True(result.Surname == _testPupil.Surname);
            Assert.True(result.LookedAfterEver);
            Assert.True(result.Ethnicity.Code == _testPupil.EthnicityCode);
            Assert.True(result.FreeSchoolMealsLast6Years);
            Assert.True(result.FirstLanguage.Code == _testPupil.FirstLanguageCode);
            Assert.True(result.ForvusIndex == int.Parse(_testPupil.ForvusIndex));
            Assert.True(result.DfesNumber == _testPupil.DFESNumber);
            Assert.True(result.PINCL.Code == _testPupil.P_INCL);
            Assert.True(result.SpecialEducationNeed.Code == _testPupil.SENStatusCode);
        }

        [Fact]
        public void WhenSearchPupilByIdShouldReturnFoundPupil1000ByUPN()
        {
            GivenRepoReturnsMultipleTestPupils();

            var result = _pupilService.QueryPupils(CheckingWindow.KS4June, new PupilsSearchRequest {ID = "11"});

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.Count == 1);
            Assert.True(result.First().Id == "1000");
        }

        [Fact]
        public void WhenSearchPupilByIdShouldReturnFoundPupil1000ByULN()
        {
            GivenRepoReturnsMultipleTestPupils();

            var result = _pupilService.QueryPupils(CheckingWindow.KS4June, new PupilsSearchRequest {ID = "101"});

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.Count == 1);
            Assert.True(result.First().Id == "1000");
        }

        [Fact]
        public void WhenSearchPupilByIdShouldReturnFoundPupil2000ByUPN()
        {
            GivenRepoReturnsMultipleTestPupils();

            var result = _pupilService.QueryPupils(CheckingWindow.KS4June, new PupilsSearchRequest {ID = "22"});

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.Count == 1);
            Assert.True(result.First().Id == "2000");
        }

        [Fact]
        public void WhenSearchPupilByIdShouldReturnFoundPupil2000ByULN()
        {
            GivenRepoReturnsMultipleTestPupils();

            var result = _pupilService.QueryPupils(CheckingWindow.KS4June, new PupilsSearchRequest {ID = "202"});

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.Count == 1);
            Assert.True(result.First().Id == "2000");
        }
        [Fact]
        public void WhenSearchPupilByNameShouldReturnMatchedPupils()
        {
            GivenRepoReturnsMultipleTestPupils();

            var result = _pupilService.QueryPupils(CheckingWindow.KS4June, new PupilsSearchRequest {Name = "Mar"});

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.Count == 2);
            Assert.True(result.First().Id == "3000");
            Assert.True(result.Last().Id == "4000");
        }

        [Fact]
        public void WhenGetPupilByIdIsCalledPupilMapsAllocationYearsToEntityObject()
        {
            GivenRepoReturnsSingleTestPupil();

            var result = _pupilService.GetById(CheckingWindow.KS4June, "100");

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

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
            GivenRepoReturnsSingleTestPupil();

            var result = _pupilService.GetById(CheckingWindow.KS4June, "100");

            _repository.Verify(x => x.Get<PupilDTO>(_ks4JunePupils), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.Results.Count == 1);
            Assert.True(result.Results.First().SubjectCode == _testPupil.performance.First().SubjectCode);
            Assert.True(result.Results.First().ExamYear == _testPupil.performance.First().ExamYear);
            Assert.True(result.Results.First().TestMark== _testPupil.performance.First().TestMark);
            Assert.True(result.Results.First().ScaledScore == _testPupil.performance.First().ScaledScore);
        }
    }
}