using System.Linq;
using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Services;
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
            _testEstab = Builder.GetEstablishment("200000");

            _repository = new Mock<IDocumentRepository>();
            _ks4JuneEstablishments = "ks4june_establishments_2021";

            _repository.Setup(x =>
                    x.GetById<EstablishmentDTO>(_ks4JuneEstablishments,
                        It.IsAny<string>()))
                .Returns(_testEstab);

            _repository.Setup(x =>
                    x.Get<EstablishmentDTO>(_ks4JuneEstablishments))
                .Returns(Builder.GetEstablisments().AsQueryable());

            _configuration = new Mock<IAllocationYearConfig>();
            _configuration.Setup(x => x.Value).Returns("2021");
        }

        private Mock<IDocumentRepository> _repository;
        private Mock<IAllocationYearConfig> _configuration;
        private EstablishmentDTO _testEstab;
        private string _ks4JuneEstablishments;

        [Fact]
        public void WhenGetEstablishmentByURNIsCalledEstablishmentDTOMapsRootFieldsToEntityObject()
        {
            var establishmentService = new EstablishmentService(_repository.Object, _configuration.Object);

            var result = establishmentService.GetByURN(CheckingWindow.KS4June, new URN("200000"));

            _repository.Verify(x => x.GetById<EstablishmentDTO>(_ks4JuneEstablishments, "200000"), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.DfesNumber == _testEstab.DFESNumber);
            Assert.True(result.Name == _testEstab.SchoolName);
            Assert.True(result.SchoolType == _testEstab.SchoolType);
            Assert.True(result.InstitutionTypeNumber == _testEstab.InstitutionTypeNumber);
            Assert.True(result.LowestAge == _testEstab.LowestAge);
            Assert.True(result.HighestAge == _testEstab.HighestAge);
            Assert.True(result.HeadTeacher == _testEstab.HeadTitleCode+ " " +_testEstab.HeadFirstName + " " +_testEstab.HeadLastName);
            Assert.True(result.Urn.Value == "200000");
        }

        [Fact]
        public void WhenGetEstablishmentByDFESNumberIsCalledEstablishmentDTOMapsRootFieldsToEntityObject()
        {
            var establishmentService = new EstablishmentService(_repository.Object, _configuration.Object);

            var result = establishmentService.GetByDFESNumber(CheckingWindow.KS4June, "DFE100000");

            _repository.Verify(x => x.Get<EstablishmentDTO>(_ks4JuneEstablishments), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.DfesNumber == "DFE100000");
            Assert.True(result.Name == _testEstab.SchoolName);
            Assert.True(result.SchoolType == _testEstab.SchoolType);
            Assert.True(result.Urn.Value == "100000");
        }

        [Fact]
        public void WhenGetEstablishmentByURNIsCalledEstablishmenMeasuresShouldMapCorrectly()
        {
            var establishmentService = new EstablishmentService(_repository.Object, _configuration.Object);

            var result = establishmentService.GetByURN(CheckingWindow.KS4June, new URN("200000"));

            _repository.Verify(x => x.GetById<EstablishmentDTO>(_ks4JuneEstablishments, "200000"), Times.Once);

            Assert.NotNull(result);
            Assert.NotNull(result.PerformanceMeasures);
            Assert.True(result.PerformanceMeasures.Count > 0);
            Assert.True(result.PerformanceMeasures.First().Name == "N01");
            Assert.True(result.PerformanceMeasures.First().Value == "V01");
        }
    }
}