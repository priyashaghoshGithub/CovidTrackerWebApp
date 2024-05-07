using CovidTrackerWebApp.Controllers;
using CovidTrackerWebApp.Models;
using CovidTrackerWebApp.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;

namespace CovidTrackerAppTest
{
    public class CovidDataWebAppTests
    {
        [Fact]
        public async void GetCovidDataAsync_SuccessResponse()
        {
            //Arrange
            //Define the Expected Covid-19 data 
            var expectedCovidData = new CovidDataModel
            {
                cases = 10000,
                recovered = 900,
                deaths =300,
                active =100,
                population =100000,
                tests =500
            };

            //Mocking the Configuration , Logger and HttpClient 
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.SetupGet(a => a["AppSettings:CovidApiUrl"]).Returns("https://disease.sh/v3/covid-19/all");
            var loggerMock = new Mock<ILogger<CovidDataService>>();
            var loggerControllerMock = new Mock<ILogger<HomeController>>();

            var responseContent = new StringContent(JsonSerializer.Serialize(expectedCovidData), Encoding.UTF8, "application/json");
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = responseContent
            };
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(responseMessage);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var httpClient = new HttpClient(handlerMock.Object);
            httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);

            var covidDataService = new CovidDataService(loggerMock.Object, httpClientFactoryMock.Object, configurationMock.Object);
            var homeController = new HomeController(loggerControllerMock.Object, covidDataService);
            //Act
            var result = await homeController.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CovidDataModel>(viewResult.Model);
            Assert.NotNull(result);
            Assert.Equal(expectedCovidData.cases, model.cases);
            Assert.Equal(expectedCovidData.deaths, model.deaths);
            Assert.Equal(expectedCovidData.active, model.active);
            Assert.Equal(expectedCovidData.tests, model.tests);
            Assert.Equal(expectedCovidData.population, model.population);
            Assert.Equal(expectedCovidData.recovered, model.recovered);
        }

        [Fact]
        public async void GetCovidDataAsync_FailureResponse()
        {
            //Arrange
            //Mocking the Configuration , Logger and HttpClient 
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.SetupGet(a => a["AppSettings:CovidApiUrl"]).Returns("test url");

            var loggerMock = new Mock<ILogger<CovidDataService>>();
            var loggerControllerMock = new Mock<ILogger<HomeController>>();

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
            var httpClientMock = new Mock<HttpClient>();
            httpClientMock.Setup(client => client.SendAsync(It.IsAny<HttpRequestMessage>(), CancellationToken.None)).ThrowsAsync(new Exception("Test Exception"));
            httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>()))
             .Returns(httpClientMock.Object);
            var covidDataService = new CovidDataService(loggerMock.Object, httpClientFactoryMock.Object, configurationMock.Object);
            var homeController = new HomeController(loggerControllerMock.Object, covidDataService);
            //Act
            var result = await homeController.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model);

        }

        [Fact]
        public async void GetCovidDataAsync_ApiUrlNull()
        {
            //Arrange
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.SetupGet(a => a["AppSettings:CovidApiUrl"]).Returns((string)null);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var loggerMock = new Mock<ILogger<CovidDataService>>();
            var loggerControllerMock = new Mock<ILogger<HomeController>>();
            var covidDataService = new CovidDataService(loggerMock.Object, httpClientFactoryMock.Object, configurationMock.Object);
            var homeController = new HomeController(loggerControllerMock.Object, covidDataService);
            //Act
            var result = await homeController.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model);
        }

        [Fact]
        public async void GetCovidDataAsync_ApiInternalServerError()
        {
            //Arrange

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.SetupGet(a => a["AppSettings:CovidApiUrl"]).Returns("https://disease.sh/v3/covid-19/all");

            var loggerMock = new Mock<ILogger<CovidDataService>>();
            var loggerControllerMock = new Mock<ILogger<HomeController>>();

            var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(responseMessage);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var httpClient = new HttpClient(handlerMock.Object);
            httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);
            var covidDataService = new CovidDataService(loggerMock.Object, httpClientFactoryMock.Object, configurationMock.Object);
            var homeController = new HomeController(loggerControllerMock.Object, covidDataService);
            //Act
            var result = await homeController.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model);

        }
    }
}