using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Entities.Dto;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Controllers;
using Xunit;

public class ProgrammeControllerTests
{
    private readonly Mock<IProgrammeService> _mockProgrammeService;
    private readonly Mock<IContactService> _mockContactService;
    private readonly ProgrammeController _controller;

    public ProgrammeControllerTests()
    {
        _mockProgrammeService = new Mock<IProgrammeService>();
        _mockContactService = new Mock<IContactService>();
        _controller = new ProgrammeController(_mockProgrammeService.Object, _mockContactService.Object);
    }

    [Fact]
    public async Task GetProgrammes_ReturnsOkResult_WithListOfProgrammeDtos()
    {
        // Arrange
        var programmes = new List<Programme>
        {
            new Programme { Id = 1, Name = "Programme1", Description = "Description1", ContactId = 1, IsActive = true },
            new Programme { Id = 2, Name = "Programme2", Description = "Description2", ContactId = 2, IsActive = false }
        };
        _mockProgrammeService.Setup(service => service.GetAllProgrammesAsync()).ReturnsAsync(programmes);

        // Act
        var result = await _controller.GetProgrammes();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnProgrammes = Assert.IsType<List<ProgrammeDto>>(okResult.Value);
        Assert.Equal(2, returnProgrammes.Count);
        Assert.Equal(programmes[0].Id, returnProgrammes[0].Id);
        Assert.Equal(programmes[0].Name, returnProgrammes[0].Name);
    }

    [Fact]
    public async Task GetProgramme_ReturnsNotFound_WhenProgrammeDoesNotExist()
    {
        // Arrange
        _mockProgrammeService.Setup(service => service.GetProgrammeByIdAsync(1)).ReturnsAsync((Programme)null);

        // Act
        var result = await _controller.GetProgramme(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetProgramme_ReturnsOkResult_WithProgrammeDto()
    {
        // Arrange
        var programme = new Programme { Id = 1, Name = "Programme1", Description = "Description1", ContactId = 1, IsActive = true };
        _mockProgrammeService.Setup(service => service.GetProgrammeByIdAsync(1)).ReturnsAsync(programme);

        // Act
        var result = await _controller.GetProgramme(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnProgramme = Assert.IsType<ProgrammeDto>(okResult.Value);
        Assert.Equal(programme.Id, returnProgramme.Id);
        Assert.Equal(programme.Name, returnProgramme.Name);
    }

    [Fact]
    public async Task CreateProgramme_ReturnsBadRequest_WhenContactIdIsInvalid()
    {
        // Arrange
        _mockContactService.Setup(service => service.GetContactById(It.IsAny<int>())).ReturnsAsync((Contact)null);

        // Act
        var result = await _controller.CreateProgramme(new ProgrammeDto { ContactId = 99 });

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Invalid ContactID", badRequestResult.Value);
    }

    [Fact]
    public async Task CreateProgramme_ReturnsCreatedAtActionResult_WhenProgrammeIsCreated()
    {
        // Arrange
        var contact = new Contact { Id = 1, Firstname = "Contact1" };
        _mockContactService.Setup(service => service.GetContactById(1)).ReturnsAsync(contact);

        var programmeDto = new ProgrammeDto { Name = "Programme1", Description = "Description1", ContactId = 1, IsActive = true };
        var programme = new Programme { Id = 1, Name = "Programme1", Description = "Description1", ContactId = 1, IsActive = true };

        _mockProgrammeService.Setup(service => service.AddProgrammeAsync(It.IsAny<Programme>()))
                             .Callback<Programme>(p => p.Id = 1)
                             .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CreateProgramme(programmeDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnProgramme = Assert.IsType<ProgrammeDto>(createdAtActionResult.Value);
        Assert.Equal(1, returnProgramme.Id);
        Assert.Equal("Programme1", returnProgramme.Name);
    }

    [Fact]
    public async Task DeleteProgramme_ReturnsNotFound_WhenProgrammeDoesNotExist()
    {
        // Arrange
        _mockProgrammeService.Setup(service => service.GetProgrammeByIdAsync(1)).ReturnsAsync((Programme)null);

        // Act
        var result = await _controller.DeleteProgramme(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteProgramme_ReturnsNoContent_WhenProgrammeIsDeleted()
    {
        // Arrange
        var programme = new Programme { Id = 1, Name = "Programme1", Description = "Description1", ContactId = 1, IsActive = true };
        _mockProgrammeService.Setup(service => service.GetProgrammeByIdAsync(1)).ReturnsAsync(programme);
        _mockProgrammeService.Setup(service => service.DeleteProgrammeAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteProgramme(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateProgramme_ReturnsBadRequest_WhenIdMismatch()
    {
        // Arrange
        var programmeDto = new ProgrammeDto { Id = 2, Name = "UpdatedProgramme", Description = "UpdatedDescription", ContactId = 1, IsActive = true };

        // Act
        var result = await _controller.UpdateProgramme(1, programmeDto);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task UpdateProgramme_ReturnsNotFound_WhenProgrammeDoesNotExist()
    {
        // Arrange
        var programmeDto = new ProgrammeDto { Id = 1, Name = "UpdatedProgramme", Description = "UpdatedDescription", ContactId = 1, IsActive = true };
        _mockProgrammeService.Setup(service => service.GetProgrammeByIdAsync(1)).ReturnsAsync((Programme)null);

        // Act
        var result = await _controller.UpdateProgramme(1, programmeDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateProgramme_ReturnsNoContent_WhenProgrammeIsUpdated()
    {
        // Arrange
        var programme = new Programme { Id = 1, Name = "Programme1", Description = "Description1", ContactId = 1, IsActive = true };
        var programmeDto = new ProgrammeDto { Id = 1, Name = "UpdatedProgramme", Description = "UpdatedDescription", ContactId = 1, IsActive = true };

        _mockProgrammeService.Setup(service => service.GetProgrammeByIdAsync(1)).ReturnsAsync(programme);
        _mockProgrammeService.Setup(service => service.UpdateProgrammeAsync(It.IsAny<Programme>())).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateProgramme(1, programmeDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
