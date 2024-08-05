using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using Xunit;

namespace TestAPI
{
    public class AuthApiControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService; // Tên biến mock này
        private readonly AuthApiController _controller;

        public AuthApiControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthApiController(_mockAuthService.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkResultWithToken()
        {
            // Arrange
            var model = new LoginViewModel { Username = "testuser", Password = "password" };
            var token = "mocked_token";
            _mockAuthService.Setup(service => service.LoginAsync(model)).ReturnsAsync(token); // Sửa tên biến ở đây

            // Act
            var result = await _controller.Login(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedValue = Assert.IsType<AuthApiController.ResponseModel>(okResult.Value);
            Assert.Equal(token, returnedValue.Token);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var model = new LoginViewModel { Username = "invaliduser", Password = "invalidpassword" };
            _mockAuthService.Setup(service => service.LoginAsync(model)).ReturnsAsync((string)null); // Sửa tên biến ở đây

            // Act
            var result = await _controller.Login(model);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var response = Assert.IsType<AuthApiController.ResponseModel>(unauthorizedResult.Value);
            Assert.Equal("Invalid username or password", response.Message);
        }

        [Fact]
        public async Task ForgotPassword_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var model = new ForgotPasswordViewModel { Username = "testuser", Email = "test@example.com" };
            _mockAuthService.Setup(service => service.ForgotPasswordAsync(model)).ReturnsAsync(true); // Sửa tên biến ở đây

            // Act
            var result = await _controller.ForgotPassword(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<AuthApiController.ResponseModel>(okResult.Value);
            Assert.Equal("Password has been sent to your email", response.Message);
        }

        [Fact]
        public async Task ForgotPassword_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var model = new ForgotPasswordViewModel { Username = "user1", Email = "invalid@example.com" };
            _mockAuthService.Setup(service => service.ForgotPasswordAsync(model)).ReturnsAsync(false); // Sửa tên biến ở đây

            // Act
            var result = await _controller.ForgotPassword(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<AuthApiController.ResponseModel>(badRequestResult.Value);
            Assert.Equal("Username and Email do not match", response.Message);
        }
    }
}
