using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using Moq;
using Xunit;
using Assert = Xunit.Assert;


namespace TestAPI.AddressTest;

    public class AddressServiceTests
    {
        private readonly Mock<IAddressRepository> _mockAddressRepository;
        private readonly AddressService _addressService;

        public AddressServiceTests()
        {
            _mockAddressRepository = new Mock<IAddressRepository>();
            _addressService = new AddressService(_mockAddressRepository.Object);
        }

        [Fact]
        public void SearchAddresses_WithAllValidInputs_ReturnsExpectedResults()
        {
            // Arrange
            var expectedResults = new List<AddressSearchResult>
            {
                new AddressSearchResult
                {
                    Address = "123 Test St",
                    PostCode = "12345",
                    Town = "TestTown",
                    County = "TestCounty",
                    CountryName = "TestCountry"
                }
            };

            _mockAddressRepository.Setup(repo => repo.Search("12345", "Test St", "TestTown"))
                .Returns(expectedResults);

            // Act
            var result = _addressService.SearchAddresses("12345", "Test St", "TestTown");

            // Assert
            Assert.Equal(expectedResults, result);
            _mockAddressRepository.Verify(repo => repo.Search("12345", "Test St", "TestTown"), Times.Once());
        }

        [Fact]
        public void SearchAddresses_WithEmptyInputs_PassesNullToRepository()
        {
            // Arrange
            _mockAddressRepository.Setup(repo => repo.Search(null, null, null))
                .Returns(new List<AddressSearchResult>());

            // Act
            var result = _addressService.SearchAddresses("", "", "");

            // Assert
            Assert.Empty(result);
            _mockAddressRepository.Verify(repo => repo.Search(null, null, null), Times.Once());
        }

        [Fact]
        public void SearchAddresses_WithNullInputs_PassesNullToRepository()
        {
            // Arrange
            _mockAddressRepository.Setup(repo => repo.Search(null, null, null))
                .Returns(new List<AddressSearchResult>());

            // Act
            var result = _addressService.SearchAddresses(null, null, null);

            // Assert
            Assert.Empty(result);
            _mockAddressRepository.Verify(repo => repo.Search(null, null, null), Times.Once());
        }

        [Fact]
        public void SearchAddresses_WithOnlyPostcode_PassesCorrectParameters()
        {
            // Arrange
            var expectedResults = new List<AddressSearchResult>
            {
                new AddressSearchResult { PostCode = "12345" }
            };

            _mockAddressRepository.Setup(repo => repo.Search("12345", null, null))
                .Returns(expectedResults);

            // Act
            var result = _addressService.SearchAddresses("12345", "", "");

            // Assert
            var resultList = result.ToList();
            Assert.Single(resultList);
            Assert.Equal("12345", resultList[0].PostCode);
            _mockAddressRepository.Verify(repo => repo.Search("12345", null, null), Times.Once());
        }

        [Fact]
        public void SearchAddresses_WithOnlyStreet_PassesCorrectParameters()
        {
            // Arrange
            var expectedResults = new List<AddressSearchResult>
            {
                new AddressSearchResult { Address = "Test St" }
            };

            _mockAddressRepository.Setup(repo => repo.Search(null, "Test St", null))
                .Returns(expectedResults);

            // Act
            var result = _addressService.SearchAddresses("", "Test St", "");

            // Assert
            var resultList = result.ToList();
            Assert.Single(resultList);
            Assert.Equal("Test St", resultList[0].Address);
            _mockAddressRepository.Verify(repo => repo.Search(null, "Test St", null), Times.Once());
        }

        [Fact]
        public void SearchAddresses_WithOnlyTown_PassesCorrectParameters()
        {
            // Arrange
            var expectedResults = new List<AddressSearchResult>
            {
                new AddressSearchResult { Town = "TestTown" }
            };

            _mockAddressRepository.Setup(repo => repo.Search(null, null, "TestTown"))
                .Returns(expectedResults);

            // Act
            var result = _addressService.SearchAddresses("", "", "TestTown");

            // Assert
            var resultList = result.ToList();
            Assert.Single(resultList);
            Assert.Equal("TestTown", resultList[0].Town);
            _mockAddressRepository.Verify(repo => repo.Search(null, null, "TestTown"), Times.Once());
        }

        [Fact]
        public void SearchAddresses_WithWhitespaceInputs_PassesNullToRepository()
        {
            // Arrange
            _mockAddressRepository.Setup(repo => repo.Search(null, null, null))
                .Returns(new List<AddressSearchResult>());

            // Act
            var result = _addressService.SearchAddresses("  ", "   ", "    ");

            // Assert
            Assert.Empty(result);
            _mockAddressRepository.Verify(repo => repo.Search(null, null, null), Times.Once());
        }

        [Fact]
        public void SearchAddresses_WithMixedValidAndEmptyInputs_PassesCorrectParameters()
        {
            // Arrange
            var expectedResults = new List<AddressSearchResult>
            {
                new AddressSearchResult { PostCode = "12345", Town = "TestTown" }
            };

            _mockAddressRepository.Setup(repo => repo.Search("12345", null, "TestTown"))
                .Returns(expectedResults);

            // Act
            var result = _addressService.SearchAddresses("12345", "", "TestTown");

            // Assert
            var resultList = result.ToList();
            Assert.Single(resultList);
            Assert.Equal("12345", resultList[0].PostCode);
            Assert.Equal("TestTown", resultList[0].Town);
            _mockAddressRepository.Verify(repo => repo.Search("12345", null, "TestTown"), Times.Once());
        }

        [Fact]
        public void SearchAddresses_RepositoryReturnsNull_ReturnsEmptyList()
        {
            // Arrange
            _mockAddressRepository.Setup(repo => repo.Search(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns((IEnumerable<AddressSearchResult>)null);

            // Act
            var result = _addressService.SearchAddresses("12345", "Test St", "TestTown");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }