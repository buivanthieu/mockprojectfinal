using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using Xunit;

namespace TestAPI;

public class GovernmentOfficeRegionControllerTests
    {
        private readonly Mock<IGovernmentOfficeRegionService> _mockService;
        private readonly GovernmentOfficeRegionController _controller;

        public GovernmentOfficeRegionControllerTests()
        {
            _mockService = new Mock<IGovernmentOfficeRegionService>();
            _controller = new GovernmentOfficeRegionController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllGovernmentOfficeRegion_ReturnsOkResult_WithListOfRegions()
        {
            // Arrange
            var expectedRegions = new List<GovernmentOfficeRegion>
            {
                new GovernmentOfficeRegion { GovernmentOfficeRegionId = 1, GovernmentOfficeRegionName = "Region 1", Description = "Description 1", CountyId = 1, IsActive = true },
                new GovernmentOfficeRegion { GovernmentOfficeRegionId = 2, GovernmentOfficeRegionName = "Region 2", Description = "Description 2", CountyId = 2, IsActive = true }
            };
            _mockService.Setup(service => service.GetAllGovernmentOfficeRegionsAsync())
                .ReturnsAsync(expectedRegions);

            // Act
            var result = await _controller.GetAllGovernmentOfficeRegion();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedRegions = Assert.IsAssignableFrom<IEnumerable<GovernmentOfficeRegion>>(okResult.Value);
            Assert.Equal(expectedRegions.Count, returnedRegions.Count());
        }

        [Fact]
        public async Task GetAllGovernmentOfficeRegion_ReturnsOkResult_WithEmptyList()
        {
            // Arrange
            var expectedRegions = new List<GovernmentOfficeRegion>();
            _mockService.Setup(service => service.GetAllGovernmentOfficeRegionsAsync())
                .ReturnsAsync(expectedRegions);

            // Act
            var result = await _controller.GetAllGovernmentOfficeRegion();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedRegions = Assert.IsAssignableFrom<IEnumerable<GovernmentOfficeRegion>>(okResult.Value);
            Assert.Empty(returnedRegions);
        }

        [Fact]
        public async Task GetGovernmentOfficeRegionById_ReturnsOkResult_WithRegion()
        {
            // Arrange
            int testId = 1;
            var expectedRegion = new GovernmentOfficeRegion 
            { 
                GovernmentOfficeRegionId = testId, 
                GovernmentOfficeRegionName = "Test Region", 
                Description = "Test Description",
                CountyId = 1,
                IsActive = true
            };
            _mockService.Setup(service => service.GetGovernmentOfficeRegionByIdAsync(testId))
                .ReturnsAsync(expectedRegion);

            // Act
            var result = await _controller.GetGovernmentOfficeRegionById(testId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedRegion = Assert.IsType<GovernmentOfficeRegion>(okResult.Value);
            Assert.Equal(expectedRegion.GovernmentOfficeRegionId, returnedRegion.GovernmentOfficeRegionId);
            Assert.Equal(expectedRegion.GovernmentOfficeRegionName, returnedRegion.GovernmentOfficeRegionName);
        }

        [Fact]
        public async Task GetGovernmentOfficeRegionById_ReturnsOkResult_WithNull()
        {
            // Arrange
            int testId = 999;
            _mockService.Setup(service => service.GetGovernmentOfficeRegionByIdAsync(testId))
                .ReturnsAsync((GovernmentOfficeRegion)null);

            // Act
            var result = await _controller.GetGovernmentOfficeRegionById(testId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async Task GetAllGovernmentOfficeRegion_ReturnsCorrectType()
        {
            // Arrange
            _mockService.Setup(service => service.GetAllGovernmentOfficeRegionsAsync())
                .ReturnsAsync(new List<GovernmentOfficeRegion>());

            // Act
            var result = await _controller.GetAllGovernmentOfficeRegion();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<GovernmentOfficeRegion>>>(result);
        }

        [Fact]
        public async Task GetGovernmentOfficeRegionById_ReturnsCorrectType()
        {
            // Arrange
            int testId = 1;
            _mockService.Setup(service => service.GetGovernmentOfficeRegionByIdAsync(testId))
                .ReturnsAsync(new GovernmentOfficeRegion 
                { 
                    GovernmentOfficeRegionId = testId, 
                    GovernmentOfficeRegionName = "Test Region", 
                    Description = "Test Description" 
                });

            // Act
            var result = await _controller.GetGovernmentOfficeRegionById(testId);

            // Assert
            Assert.IsType<ActionResult<GovernmentOfficeRegion>>(result);
        }
    }