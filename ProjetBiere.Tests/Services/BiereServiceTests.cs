using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjetBiere.Entity;
using ProjetBiere.Repository;
using ProjetBiere.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetBiere.Test.Services
{
    [TestClass]
    public class BiereServiceTests
    {
        private IFixture _fixture;
        private Mock<IBiereRepository> _mockBiereRepository;
        private BiereService _biereService;

        [TestInitialize]
        public void Initialize()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mockBiereRepository = _fixture.Freeze<Mock<IBiereRepository>>();
            _biereService = _fixture.Create<BiereService>();
        }
        

        #region GetBiere()

        [TestMethod]
        public async Task GetBiereAvecCorrespondanceAlorsRetourneBieres()
        {
            // Arrange
            var bieresAttendues = _fixture.CreateMany<Biere>();
            _mockBiereRepository.Setup(x => x.GetBiere()).ReturnsAsync(bieresAttendues);

            // Act
            var bieres = await _biereService.GetBiere();

            // Assert
            Assert.AreEqual(bieresAttendues, bieres);
        }


        [TestMethod]
        public async Task GetBiereSansCorrespondanceAlorsRetourneNull()
        {
            // Arrange
            _mockBiereRepository.Setup(x => x.GetBiere()).ReturnsAsync((List<Biere>)null);

            // Act
            var bieres = await _biereService.GetBiere();

            // Assert
            Assert.IsNull(bieres);

        }

        #endregion

        #region GetBiere(biereId)
        [TestMethod]
        public async Task GetBiereAvecIdAlorsRetourneBiere()
        {
            //Arrange
            var biereAttendue = _fixture.Create<Biere>();
            _mockBiereRepository.Setup(x => x.GetBiere(biereAttendue.Id)).ReturnsAsync(biereAttendue);

            //Act
            var biere = await _biereService.GetBiere(biereAttendue.Id);

            //Assert
            Assert.AreEqual(biereAttendue, biere);
        }

        [TestMethod]
        public async Task GetBiereAvecIdInvalideAlorsRetourneNull()
        {
            //Arrange
            var biere = _fixture.Create<Biere>();
            _mockBiereRepository.Setup(x => x.GetBiere(It.IsAny<int>())).ReturnsAsync((Biere)null);

            //Act
            var result = await _biereService.GetBiere(biere.Id);

            //Assert
            Assert.IsNull(result);
        }
        #endregion

        #region Post(biere)


        [TestMethod]
        public async Task PostUneBiereAvecSuccesAlorsRetourneBiere()
        {
            // Arrange
            Biere biere = _fixture.Create<Biere>();
            _mockBiereRepository.Setup(x => x.Post(It.IsAny<Biere>())).ReturnsAsync(biere);

            // Act
            var biereRetounee = await _biereService.Post(biere);

            // Assert
            Assert.AreEqual(biere, biereRetounee);
        }

        [TestMethod]
        public async Task PostUneBiereEnEchecAlorsRetourneNull()
        {
            // Arrange
            Biere biere = _fixture.Create<Biere>();
            _mockBiereRepository.Setup(x => x.Post(It.IsAny<Biere>())).ReturnsAsync((Biere)null);

            // Act
            object biereRetournee = await _biereService.Post(biere);

            // Assert
            Assert.IsNull(biereRetournee);
        }


        #endregion

    }
}
