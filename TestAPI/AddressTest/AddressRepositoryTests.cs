using BusinessLogicLayer.Repositories;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Assert = Xunit.Assert;


namespace TestAPI.AddressTest;

public class AddressRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly AddressRepository _repository;

        public AddressRepositoryTests()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new AddressRepository(_context);

            // Seed the database
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var country = new Country { CountryID = 1, CountryName = "TestCountry" };
            var county = new County { CountyID = 1, CountryID = 1, CountyName = "TestCounty", Country = country };
            var town = new Town { TownID = 1, CountyID = 1, TownName = "TestTown", County = county };
            var address1 = new Address { AddressID = 1, AddressName = "123 Test St", PostCode = "12345", TownID = 1, Town = town };
            var address2 = new Address { AddressID = 2, AddressName = "456 Sample Rd", PostCode = "67890", TownID = 1, Town = town };

            _context.Countries.Add(country);
            _context.Counties.Add(county);
            _context.Towns.Add(town);
            _context.Addresses.AddRange(address1, address2);
            _context.SaveChanges();
        }

        [Fact]
        public void Search_WithValidPostcode_ReturnsCorrectResult()
        {
            // Act
            var result = _repository.Search("12345", null, null).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("123 Test St", result[0].Address);
            Assert.Equal("12345", result[0].PostCode);
            Assert.Equal("TestTown", result[0].Town);
            Assert.Equal("TestCounty", result[0].County);
            Assert.Equal("TestCountry", result[0].CountryName);
        }

        [Fact]
        public void Search_WithValidStreet_ReturnsCorrectResult()
        {
            // Act
            var result = _repository.Search(null, "Test St", null).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("123 Test St", result[0].Address);
        }

        [Fact]
        public void Search_WithValidTown_ReturnsAllAddressesInTown()
        {
            // Act
            var result = _repository.Search(null, null, "TestTown").ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.Address == "123 Test St");
            Assert.Contains(result, r => r.Address == "456 Sample Rd");
        }

        [Fact]
        public void Search_WithNonExistentPostcode_ReturnsEmptyResult()
        {
            // Act
            var result = _repository.Search("99999", null, null);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Search_WithMultipleParameters_ReturnsCorrectResult()
        {
            // Act
            var result = _repository.Search("12345", "Test St", "TestTown").ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("123 Test St", result[0].Address);
            Assert.Equal("12345", result[0].PostCode);
            Assert.Equal("TestTown", result[0].Town);
        }

        [Fact]
        public void Search_WithPartialStreetName_ReturnsCorrectResult()
        {
            // Act
            var result = _repository.Search(null, "Test", null).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("123 Test St", result[0].Address);
        }

        [Fact]
        public void Search_WithAllNullParameters_ReturnsAllAddresses()
        {
            // Act
            var result = _repository.Search(null, null, null).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.Address == "123 Test St");
            Assert.Contains(result, r => r.Address == "456 Sample Rd");
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }