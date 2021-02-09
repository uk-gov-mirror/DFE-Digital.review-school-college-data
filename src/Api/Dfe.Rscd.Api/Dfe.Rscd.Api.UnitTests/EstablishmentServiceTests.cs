using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories;
using Dfe.Rscd.Api.Services;
using Moq;
using Xunit;

namespace Dfe.Rscd.Api.UnitTests
{
    public class EstablishmentServiceTests
    {
        public EstablishmentServiceTests()
        {
            Given();
        }
        
        private void Given()
        {
            _testUrn = "200000";
            _testEstab = Builder.GetEstablishment(_testUrn);

            _repository = new Mock<IDocumentRepository>();

            _repository.Setup(x =>
                    x.Get<EstablishmentDTO>(_ks4JuneEstablishments))
                .Returns(new List<EstablishmentDTO> { _testEstab }.AsQueryable());

            _configuration = new Mock<IAllocationYearConfig>();
            _configuration.Setup(x => x.Value).Returns("2021");
        }

        private Mock<IDocumentRepository> _repository;
        private Mock<IAllocationYearConfig> _configuration;
        private EstablishmentDTO _testEstab;
        private const string _ks4JuneEstablishments = "ks4june_establishments_2021";
        private string _testUrn;

        [Fact]
        public void WhenGetEstablishmentByURNIsCalledEstablishmentDTOMapsRootFieldsToEntityObject()
        {
            var establishmentService = new EstablishmentService(_repository.Object, _configuration.Object);

            var result = establishmentService.GetByURN(CheckingWindow.KS4June, new URN(_testUrn));

            _repository.Verify(x => x.Get<EstablishmentDTO>(_ks4JuneEstablishments), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.DfesNumber.ToString() == _testEstab.DFESNumber);
            Assert.True(result.SchoolName == _testEstab.SchoolName);
            Assert.True(result.SchoolType == _testEstab.SchoolType);
            Assert.True(result.InstitutionTypeNumber == _testEstab.InstitutionTypeNumber);
            Assert.True(result.LowestAge == _testEstab.LowestAge);
            Assert.True(result.HighestAge == _testEstab.HighestAge);
            Assert.True(result.HeadTeacher == _testEstab.HeadTitleCode+ " " +_testEstab.HeadFirstName + " " +_testEstab.HeadLastName);
            Assert.True(result.Urn.Value == _testUrn);
        }

        [Fact]
        public void WhenGetEstablishmentByDFESNumberIsCalledEstablishmentDTOMapsRootFieldsToEntityObject()
        {
            var establishmentService = new EstablishmentService(_repository.Object, _configuration.Object);

            var result = establishmentService.GetByDFESNumber(CheckingWindow.KS4June, $"99{_testUrn}");

            _repository.Verify(x => x.Get<EstablishmentDTO>(_ks4JuneEstablishments), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.DfesNumber == int.Parse($"99{_testUrn}"));
            Assert.True(result.SchoolName == _testEstab.SchoolName);
            Assert.True(result.SchoolType == _testEstab.SchoolType);
            Assert.True(result.Urn.Value == _testUrn);
        }

        [Fact]
        public void WhenGetEstablishmentByURNIsCalledEstablishmenMeasuresShouldMapCorrectly()
        {
            var establishmentService = new EstablishmentService(_repository.Object, _configuration.Object);

            var result = establishmentService.GetByURN(CheckingWindow.KS4June, new URN(_testUrn));

            _repository.Verify(x => x.Get<EstablishmentDTO>(_ks4JuneEstablishments), Times.Once);

            Assert.NotNull(result);
            Assert.NotNull(result.PerformanceMeasures);
            Assert.True(result.PerformanceMeasures.Count > 0);
            Assert.True(result.PerformanceMeasures.First().Name == "N01");
            Assert.True(result.PerformanceMeasures.First().Value == "V01");
        }
    }
}