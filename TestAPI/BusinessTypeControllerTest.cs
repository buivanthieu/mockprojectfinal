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
    public class BusinessTypeControllerTest
    {
        private BusinessTypeController _controller;
        private Mock<IBusinessTypeService> _businessTypeServiceMock;

        [SetUp]
        public void Setup()
        {
            _businessTypeServiceMock = new Mock<IBusinessTypeService>();
            _controller = new BusinessTypeController(_businessTypeServiceMock.Object);
        }

        // [Test]
        // public void Search_ReturnsCorrectBusinessTypeDetails()
        // {
        //     // Arrange
        //     var businessTypes = new List<BusinessType>
        //     {
        //         new BusinessType { BusinessID = 1, BusinessName = "Retail", SICCode = "1234" },
        //         new BusinessType { BusinessID = 2, BusinessName = "Wholesale", SICCode = "5678" }
        //     };

        //     _businessTypeServiceMock.Setup(service => service.SearchBusinessTypes("Retail", "1234"))
        //         .Returns(businessTypes);

        //     // Act
        //     var result = _controller.Search("Retail", "1234") as OkObjectResult;

        //     // Assert
        //     Assert.IsNotNull(result, "Result is not OkObjectResult");
        //     var returnValue = result?.Value as IEnumerable<BusinessType>;
        //     Assert.IsNotNull(returnValue, "Value is not IEnumerable<BusinessType>");
        //     Assert.That(returnValue.Count(), Is.EqualTo(2));
        //     Assert.That(returnValue.First().BusinessID, Is.EqualTo(1));
        //     Assert.That(returnValue.First().BusinessName, Is.EqualTo("Retail"));
        //     Assert.That(returnValue.First().SICCode, Is.EqualTo("1234"));
        // }

        // [Test]
        // public void Search_ReturnsEmptyList_WhenNoBusinessTypesMatch()
        // {
        //     // Arrange
        //     _businessTypeServiceMock.Setup(service => service.SearchBusinessTypes("Nonexistent", "9999"))
        //         .Returns(new List<BusinessType>());

        //     // Act
        //     var result = _controller.Search("Nonexistent", "9999") as OkObjectResult;

        //     // Assert
        //     Assert.IsNotNull(result, "Result is not OkObjectResult");
        //     var returnValue = result?.Value as IEnumerable<BusinessType>;
        //     Assert.IsNotNull(returnValue, "Value is not IEnumerable<BusinessType>");
        //     Assert.IsEmpty(returnValue);
        // }
        [Test]
        public void Search_ReturnsNotFound_WhenNoBusinessTypesMatch()
        {
            // Arrange
            _businessTypeServiceMock.Setup(service => service.SearchBusinessTypes("Nonexistent", "9999"))
                .Returns(new List<BusinessType>());

            // Act
            var result = _controller.Search("Nonexistent", "9999") as ActionResult<IEnumerable<BusinessType>>;

            // Assert
            Assert.IsNotNull(result, "Result is null");
            var notFoundResult = result.Result as NotFoundResult;
            Assert.IsNotNull(notFoundResult, "Result is not NotFoundResult");
        }

    }
}
