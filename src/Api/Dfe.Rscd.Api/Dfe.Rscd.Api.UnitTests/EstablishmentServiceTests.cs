using System.Linq;
using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Infrastructure;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Dfe.Rscd.Api.UnitTests
{
    [TestFixture]
    public class EstablishmentServiceTests
    {
        [SetUp]
        public void Given()
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

            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(x => x["AllocationYear"]).Returns("2021");
        }

        private Mock<IDocumentRepository> _repository;
        private Mock<IConfiguration> _configuration;
        private EstablishmentDTO _testEstab;
        private string _ks4JuneEstablishments;

        [Test]
        public void WhenGetEstablishmentByURNIsCalledEstablishmentDTOMapsRootFieldsToEntityObject()
        {
            var establishmentService = new EstablishmentService(_repository.Object, _configuration.Object);

            var result = establishmentService.GetByURN(CheckingWindow.KS4June, new URN("200000"));

            _repository.Verify(x => x.GetById<EstablishmentDTO>(_ks4JuneEstablishments, "200000"), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.DfesNumber, Is.EqualTo(_testEstab.DFESNumber));
            Assert.That(result.Name, Is.EqualTo(_testEstab.SchoolName));
            Assert.That(result.SchoolType, Is.EqualTo(_testEstab.SchoolType));
            Assert.That(result.InstitutionTypeNumber, Is.EqualTo(_testEstab.InstitutionTypeNumber));
            Assert.That(result.LowestAge, Is.EqualTo(_testEstab.LowestAge));
            Assert.That(result.HighestAge, Is.EqualTo(_testEstab.HighestAge));
            Assert.That(result.HeadTeacher, Is.EqualTo(_testEstab.HeadTitleCode+ " " +_testEstab.HeadFirstName + " " +_testEstab.HeadLastName));
            Assert.That(result.Urn.Value, Is.EqualTo("200000"));
        }

        [Test]
        public void WhenGetEstablishmentByDFESNumberIsCalledEstablishmentDTOMapsRootFieldsToEntityObject()
        {
            var establishmentService = new EstablishmentService(_repository.Object, _configuration.Object);

            var result = establishmentService.GetByDFESNumber(CheckingWindow.KS4June, "DFE100000");

            _repository.Verify(x => x.Get<EstablishmentDTO>(_ks4JuneEstablishments), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.DfesNumber, Is.EqualTo("DFE100000"));
            Assert.That(result.Name, Is.EqualTo(_testEstab.SchoolName));
            Assert.That(result.SchoolType, Is.EqualTo(_testEstab.SchoolType));
            Assert.That(result.Urn.Value, Is.EqualTo("100000"));
        }

        [Test]
        public void WhenGetEstablishmentByURNIsCalledEstablishmenMeasuresShouldMapCorrectly()
        {
            var establishmentService = new EstablishmentService(_repository.Object, _configuration.Object);

            var result = establishmentService.GetByURN(CheckingWindow.KS4June, new URN("200000"));

            _repository.Verify(x => x.GetById<EstablishmentDTO>(_ks4JuneEstablishments, "200000"), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.PerformanceMeasures, Is.Not.Null.Or.Empty);
            Assert.That(result.PerformanceMeasures.First().Name, Is.EqualTo("N01"));
            Assert.That(result.PerformanceMeasures.First().Value, Is.EqualTo("V01"));
        }
    }
}