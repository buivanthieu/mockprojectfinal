using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace TestAPI
{
    [TestFixture]
    public class BusinessTypeControllerTests
    {
        private BusinessTypeController _controller;
        private Mock<IBusinessTypeService> _businessTypeServiceMock;

        [SetUp]
        public void Setup()
        {
            _businessTypeServiceMock = new Mock<IBusinessTypeService>();
            _controller = new BusinessTypeController(_businessTypeServiceMock.Object);
        }

        [Test]
        public void Search_ReturnsCorrectBusinessTypeDetails()
        {
            // Arrange
            var businessTypes = new List<BusinessType>
            {
                new BusinessType { BusinessID = 1, BusinessName = "Retail", SICCode = "1234" },
                new BusinessType { BusinessID = 2, BusinessName = "Wholesale", SICCode = "5678" }
            };

            _businessTypeServiceMock.Setup(service => service.SearchBusinessTypes("Retail", "1234"))
                .Returns(businessTypes);

            // Act
            var result = _controller.Search("Retail", "1234");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var returnValue = okResult.Value as IEnumerable<BusinessType>;
            Assert.IsNotNull(returnValue);
            Assert.That(returnValue.Count(), Is.EqualTo(2));
            Assert.That(returnValue.First().BusinessID, Is.EqualTo(1));
            Assert.That(returnValue.First().BusinessName, Is.EqualTo("Retail"));
            Assert.That(returnValue.First().SICCode, Is.EqualTo("1234"));
        }

        [Test]
        public void Search_ReturnsNotFound_WhenNoBusinessTypesMatch()
        {
            // Arrange
            _businessTypeServiceMock.Setup(service => service.SearchBusinessTypes("Nonexistent", "9999"))
                .Returns(new List<BusinessType>());

            // Act
            var result = _controller.Search("Nonexistent", "9999");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var returnValue = okResult.Value as IEnumerable<BusinessType>;
            Assert.IsNotNull(returnValue);
            Assert.IsEmpty(returnValue);
        }

        [Test]
        public void Search_ReturnsCorrectBusinessType_WhenOnlyBusinessNameProvided()
        {
            // Arrange
            var businessTypes = new List<BusinessType>
            {
                new BusinessType { BusinessID = 1, BusinessName = "Retail", SICCode = "1234" }
            };

            _businessTypeServiceMock.Setup(service => service.SearchBusinessTypes("Retail", ""))
                .Returns(businessTypes);

            // Act
            var result = _controller.Search("Retail", "");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var returnValue = okResult.Value as IEnumerable<BusinessType>;
            Assert.IsNotNull(returnValue);
            Assert.That(returnValue.Count(), Is.EqualTo(1));
            Assert.That(returnValue.First().BusinessName, Is.EqualTo("Retail"));
        }

        [Test]
        public void Search_ReturnsCorrectBusinessType_WhenOnlySicCodeProvided()
        {
            // Arrange
            var businessTypes = new List<BusinessType>
            {
                new BusinessType { BusinessID = 1, BusinessName = "Retail", SICCode = "1234" }
            };

            _businessTypeServiceMock.Setup(service => service.SearchBusinessTypes("", "1234"))
                .Returns(businessTypes);

            // Act
            var result = _controller.Search("", "1234");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var returnValue = okResult.Value as IEnumerable<BusinessType>;
            Assert.IsNotNull(returnValue);
            Assert.That(returnValue.Count(), Is.EqualTo(1));
            Assert.That(returnValue.First().SICCode, Is.EqualTo("1234"));
        }

        [Test]
        public void Search_ReturnsEmptyResult_WhenNoParametersProvided()
        {
            // Arrange
            var businessTypes = new List<BusinessType>();

            _businessTypeServiceMock.Setup(service => service.SearchBusinessTypes("", ""))
                .Returns(businessTypes);

            // Act
            var result = _controller.Search("", "");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var returnValue = okResult.Value as IEnumerable<BusinessType>;
            Assert.IsNotNull(returnValue);
            Assert.IsEmpty(returnValue);
        }

      
    }
}
