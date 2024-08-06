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
    public class AddressControllerTests
    {
        private AddressController _controller;
        private Mock<IAddressService> _addressServiceMock;

        [SetUp]
        public void Setup()
        {
            _addressServiceMock = new Mock<IAddressService>();
            _controller = new AddressController(_addressServiceMock.Object);
        }

        [Test]
        public void Search_ReturnsCorrectAddressDetails()
        {
            // Arrange
            var addresses = new List<AddressSearchResult>
            {
                new AddressSearchResult { Address = "123 Test St", PostCode = "12345", Town = "TestTown", County = "TestCounty", CountryName = "TestCountry" }
            };

            _addressServiceMock.Setup(service => service.SearchAddresses("12345", "Test St", "TestTown"))
                .Returns(addresses);

            // Act
            var result = _controller.Search("12345", "Test St", "TestTown");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var returnValue = okResult.Value as IEnumerable<AddressSearchResult>;
            Assert.IsNotNull(returnValue);
            Assert.That(returnValue.Count(), Is.EqualTo(1));
            var address = returnValue.First();
            Assert.That(address.Address, Is.EqualTo("123 Test St"));
            Assert.That(address.PostCode, Is.EqualTo("12345"));
            Assert.That(address.Town, Is.EqualTo("TestTown"));
            Assert.That(address.County, Is.EqualTo("TestCounty"));
            Assert.That(address.CountryName, Is.EqualTo("TestCountry"));
        }

        [Test]
        public void Search_ReturnsNotFound_WhenNoAddressesMatch()
        {
            // Arrange
            _addressServiceMock.Setup(service => service.SearchAddresses("Nonexistent", "9999", "NonexistentTown"))
                .Returns(new List<AddressSearchResult>());

            // Act
            var result = _controller.Search("Nonexistent", "9999", "NonexistentTown");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var returnValue = okResult.Value as IEnumerable<AddressSearchResult>;
            Assert.IsNotNull(returnValue);
            Assert.IsEmpty(returnValue);
        }

        [Test]
        public void Search_ReturnsCorrectAddress_WhenOnlyPostcodeProvided()
        {
            // Arrange
            var addresses = new List<AddressSearchResult>
            {
                new AddressSearchResult { Address = "123 Test St", PostCode = "12345", Town = "TestTown", County = "TestCounty", CountryName = "TestCountry" }
            };

            _addressServiceMock.Setup(service => service.SearchAddresses("12345", "", ""))
                .Returns(addresses);

            // Act
            var result = _controller.Search("12345", "", "");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var returnValue = okResult.Value as IEnumerable<AddressSearchResult>;
            Assert.IsNotNull(returnValue);
            Assert.That(returnValue.Count(), Is.EqualTo(1));
            Assert.That(returnValue.First().PostCode, Is.EqualTo("12345"));
        }

        [Test]
        public void Search_ReturnsCorrectAddress_WhenOnlyStreetProvided()
        {
            // Arrange
            var addresses = new List<AddressSearchResult>
            {
                new AddressSearchResult { Address = "123 Test St", PostCode = "12345", Town = "TestTown", County = "TestCounty", CountryName = "TestCountry" }
            };

            _addressServiceMock.Setup(service => service.SearchAddresses("", "Test St", ""))
                .Returns(addresses);

            // Act
            var result = _controller.Search("", "Test St", "");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var returnValue = okResult.Value as IEnumerable<AddressSearchResult>;
            Assert.IsNotNull(returnValue);
            Assert.That(returnValue.Count(), Is.EqualTo(1));
            Assert.That(returnValue.First().Address, Is.EqualTo("123 Test St"));
        }

        [Test]
        public void Search_ReturnsCorrectAddress_WhenOnlyTownProvided()
        {
            // Arrange
            var addresses = new List<AddressSearchResult>
            {
                new AddressSearchResult { Address = "123 Test St", PostCode = "12345", Town = "TestTown", County = "TestCounty", CountryName = "TestCountry" }
            };

            _addressServiceMock.Setup(service => service.SearchAddresses("", "", "TestTown"))
                .Returns(addresses);

            // Act
            var result = _controller.Search("", "", "TestTown");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var returnValue = okResult.Value as IEnumerable<AddressSearchResult>;
            Assert.IsNotNull(returnValue);
            Assert.That(returnValue.Count(), Is.EqualTo(1));
            Assert.That(returnValue.First().Town, Is.EqualTo("TestTown"));
        }

        [Test]
        public void Search_ReturnsEmptyResult_WhenNoParametersProvided()
        {
            // Arrange
            var addresses = new List<AddressSearchResult>();

            _addressServiceMock.Setup(service => service.SearchAddresses("", "", ""))
                .Returns(addresses);

            // Act
            var result = _controller.Search("", "", "");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var returnValue = okResult.Value as IEnumerable<AddressSearchResult>;
            Assert.IsNotNull(returnValue);
            Assert.IsEmpty(returnValue);
        }
    }
}
