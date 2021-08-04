using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjetBiere.Controllers;
using ProjetBiere.Entity;
using ProjetBiere.Modeles;
using ProjetBiere.Services;

namespace ProjetBiere.Tests.Controllers
{
    [TestClass]
    public class BiereControllerTest
    {
        private IFixture _fixture;
        private Mock<IBiereService> _mockBiereService;
        private Mock<LinkGenerator> _mockLinkGenerator;
        private Mock<IMapper> _mockMapper;
        private BiereController _biereController;
        private string _baseURL;

        [TestInitialize]
        public void Initialize()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Customize<BindingInfo>(c => c.OmitAutoProperties());
            _mockBiereService = _fixture.Freeze<Mock<IBiereService>>();
            _mockLinkGenerator = _fixture.Freeze<Mock<LinkGenerator>>();
            _mockMapper = _fixture.Freeze<Mock<IMapper>>();
            _biereController = _fixture.Create<BiereController>();
            _baseURL = "/api/v1/Biere";
        }


        #region Get()

        [TestMethod]
        public async Task GetBiereAvecCorespondanceAlorsRetourneListDeBiereModele()
        {
            // Arrange
            var bieresAttendues = _fixture.CreateMany<Biere>();
            var bieresModele = _fixture.CreateMany<BiereModele>();
            _mockBiereService.Setup(x => x.GetBiere()).ReturnsAsync(bieresAttendues);
            _mockMapper.Setup(x => x.Map<IEnumerable<BiereModele>>(It.IsAny<IEnumerable<Biere>>())).Returns(bieresModele);

            // Act
            var actionResult = await _biereController.Get();
            var okResult = actionResult.Result as OkObjectResult;
            var bieres = okResult.Value as IEnumerable<BiereModele>;

            // Assert
            Assert.AreEqual(bieresModele, bieres);
        }

        [TestMethod]
        public async Task GetBiereAvecCorrespondanceAlorsRetourneOk200()
        {
            // Arrange
            var bieres = _fixture.CreateMany<Biere>();
            _mockBiereService.Setup(x => x.GetBiere()).ReturnsAsync(bieres);

            // Act
            var actionResult = await _biereController.Get();

            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetBiereSansCorrespondanceAlorsRetourneNotFound404()
        {
            //Arrange
            _mockBiereService.Setup(x => x.GetBiere()).ReturnsAsync((List<Biere>)null);

            //Act
            var actionResult = await _biereController.Get();

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetBiereAlorsRetourneBadRequest400()
        {
            //Arrange
            _mockBiereService.Setup(x => x.GetBiere()).ThrowsAsync(new Exception());

            //Act
            var actionResult = await _biereController.Get();

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestObjectResult));
        }

        #endregion

        #region Get(biereID)

        [TestMethod]
        public async Task GetBiereAvecIdAvecCorrespondanceAlorsRetourneBiere()
        {
            // Arrange
            var biereAttendue = _fixture.Create<Biere>();
            var biereModele = _fixture.Create<BiereModele>();
            _mockBiereService.Setup(x => x.GetBiere(biereAttendue.Id)).ReturnsAsync(biereAttendue);
            _mockMapper.Setup(x => x.Map<BiereModele>(It.IsAny<Biere>())).Returns(biereModele);

            // Act
            var actionResult = await _biereController.Get(biereAttendue.Id);
            var okResult = actionResult.Result as OkObjectResult;
            var biere = okResult.Value as BiereModele;

            // Assert
            Assert.AreEqual(biereModele, biere);
        }

        [TestMethod]
        public async Task GetBiereAvecIdAvecCorespondanceAlorsRetourneOk200()
        {
            //Arrange
            var biere = _fixture.Create<Biere>();
            _mockBiereService.Setup(x => x.GetBiere(biere.Id)).ReturnsAsync(biere);

            //Act
            var actionResult = await _biereController.Get(biere.Id);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(OkObjectResult));
        }


        [TestMethod]
        public async Task GetBiereAvecIdSansCorrespondanceAlorsRetourneNotFound404()
        {
            //Arrange
            var biere = _fixture.Create<Biere>();
            _mockBiereService.Setup(x => x.GetBiere(It.IsAny<int>())).ReturnsAsync((Biere)null);

            //Act
            var actionResult = await _biereController.Get(biere.Id);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetBiereAvecExceptionAlorsRetourneBadRequest400()
        {
            //Arrange
            var biere = _fixture.Create<Biere>();
            _mockBiereService.Setup(x => x.GetBiere(It.IsAny<int>())).ThrowsAsync(new Exception());

            //Act
            var actionResult = await _biereController.Get(biere.Id);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestObjectResult));
        }

        #endregion

        #region Post()

        [TestMethod]
        public async Task PostUneBiereAvecSuccesAlorsRetourneBiereSauvegardee()
        {
            // Arrange
            var biereModele = _fixture.Create<BiereModele>();
            var biere = _fixture.Create<Biere>();

            _mockBiereService.Setup(x => x.GetBiere(It.IsAny<int>())).ReturnsAsync((Biere)null);
            _mockBiereService.Setup(x => x.Post(It.IsAny<Biere>())).ReturnsAsync(biere);
            InititialiserMockLinkGenerator($"{_baseURL}/{biere.Id}");
            _mockMapper.Setup(x => x.Map<BiereModele>(It.IsAny<Biere>())).Returns(biereModele);

            // Act
            var actionResult = await _biereController.Post(biereModele);
            var createdResult = actionResult.Result as CreatedResult;
            var biereRetournee = createdResult.Value as BiereModele;

            // Assert
            Assert.AreEqual(biereModele, biereRetournee);
        }

        [TestMethod]
        public async Task PostUneBiereAvecSuccesAlorsReturneCreated201()
        {
            // Arrange
            var biereModele = _fixture.Create<BiereModele>();
            var biere = _fixture.Create<Biere>();
            _mockBiereService.Setup(x => x.GetBiere(It.IsAny<int>())).ReturnsAsync((Biere)null);
            InititialiserMockLinkGenerator($"{_baseURL}/{biere.Id}");

            // Act
            var actionResult = await _biereController.Post(biereModele);

            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedResult));
        }

        //[TestMethod]
        //public async Task PostUneBiereAvecSuccesAlorsRetourneURLAvecBiereID()
        //{
        //    // Arrange
        //    Biere biere = _fixture.Create<Biere>();
        //    _mockBiereService.Setup(x => x.GetBiere(It.IsAny<int>())).ReturnsAsync((Biere)null);
        //    _mockBiereService.Setup(x => x.Post(It.IsAny<Biere>())).ReturnsAsync(biere);
        //    InititialiserMockLinkGenerator($"{_baseURL}/{biere.Id}");

        //    // Act
        //    var createdResult = await _biereController.Post(biere);

        //    // Assert
        //    //Assert.AreEqual($"{_baseURL}/{biere.Id}", urlRetournee);
        //    _mockLinkGenerator.Verify(x => x.GetPathByAction("Get", "Biere", biere.Id, It.IsAny<PathString>(), It.IsAny<FragmentString>(), It.IsAny<LinkOptions>()));
        //}

        [TestMethod]
        public async Task PostUneBiereDejaUtiliseAlorsRetrouneBadRequest400()
        {
            // Arrange
            var biereModele = _fixture.Create<BiereModele>();

            var biere = _fixture.Create<Biere>();
            _mockBiereService.Setup(x => x.GetBiere(It.IsAny<int>())).ReturnsAsync(biere);

            // Act
            var actionResult = await _biereController.Post(biereModele);

            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task PostUneBiereDejaUtiliseAlorsRetrouneMessage()
        {
            // Arrange            
            var biereModele = _fixture.Create<BiereModele>();
            var biere = _fixture.Create<Biere>();
            _mockBiereService.Setup(x => x.GetBiere(It.IsAny<int>())).ReturnsAsync(biere);

            // Act
            var actionResult = await _biereController.Post(biereModele);
            var badRequestResult = actionResult.Result as BadRequestObjectResult;
            var message = badRequestResult.Value as string;

            // Assert
            Assert.AreEqual("Biere déjà sauvegardée", message);
        }

        [TestMethod]
        public async Task PostUneBiereAvecUneMauvaisURLAlorsRetourneBadRequest400()
        {
            // Arrange
            var biereModele = _fixture.Create<BiereModele>();
            var biere = _fixture.Create<Biere>();
            _mockBiereService.Setup(x => x.GetBiere(It.IsAny<int>())).ReturnsAsync((Biere)null);
            InititialiserMockLinkGenerator(null);

            // Act
            var actionResult = await _biereController.Post(biereModele);

            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task PostUneBiereAvecUneMauvaisURLAlorsRetourneMessage()
        {
            // Arrange
            var biereModele = _fixture.Create<BiereModele>();
            var biere = _fixture.Create<Biere>();
            _mockBiereService.Setup(x => x.GetBiere(It.IsAny<int>())).ReturnsAsync((Biere)null);
            InititialiserMockLinkGenerator(null);

            // Act
            var actionResult = await _biereController.Post(biereModele);
            var badRequestResult = actionResult.Result as BadRequestObjectResult;
            var message = badRequestResult.Value as string;

            // Assert
            Assert.AreEqual("Mauvais URL", message);
        }

        [TestMethod]
        public async Task PostUneBiereAvecExceptionAlorsRetourneBadRequest400()
        {
            // Arrange
            var biereModele = _fixture.Create<BiereModele>();
            var biere = _fixture.Create<Biere>();
            _mockBiereService.Setup(x => x.GetBiere(It.IsAny<int>())).ReturnsAsync((Biere)null);
            _mockBiereService.Setup(x => x.Post(It.IsAny<Biere>())).ThrowsAsync(new Exception());
            InititialiserMockLinkGenerator($"{_baseURL}/{biere.Id}");

            // Act
            var actionResult = await _biereController.Post(biereModele);

            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestObjectResult));
        }

        #endregion


        #region Méthodes privées

        private void InititialiserMockLinkGenerator(string path)
        {
            _mockLinkGenerator.Setup(g => g.GetPathByAddress(It.IsAny<RouteValuesAddress>(),
                                                             It.IsAny<RouteValueDictionary>(),
                                                             It.IsAny<PathString>(),
                                                             It.IsAny<FragmentString>(),
                                                             It.IsAny<LinkOptions>()))
                                                             .Returns(path);
        }


        #endregion

    }
}
