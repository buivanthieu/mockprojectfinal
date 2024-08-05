using BusinessLogicLayer.Repositories;
using BusinessLogicLayer.Services;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Controllers;
using Xunit;
using Assert = Xunit.Assert;


namespace TestAPI;

public sealed class AddressControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly AddressController _controller;
        private bool _disposed = false;
        public AddressControllerTests()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            // Set up the repository and service
            var repository = new AddressRepository(_context);
            var service = new AddressService(repository);

            // Set up the controller
            _controller = new AddressController(service);

            // Seed the database
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var country = new Country { CountryID = 1, CountryName = "TestCountry" };
            var county = new County { CountyID = 1, CountryID = 1, CountyName = "TestCounty", Country = country };
            var town = new Town { TownID = 1, CountyID = 1, TownName = "TestTown", County = county };
            var address = new Address { AddressID = 1, AddressName = "123 Test St", PostCode = "12345", TownID = 1, Town = town };

            _context.Countries.Add(country);
            _context.Counties.Add(county);
            _context.Towns.Add(town);
            _context.Addresses.Add(address);
            _context.SaveChanges();
        }

        [Fact]
        public void Search_WithValidPostcode_ReturnsCorrectResult()
        {
            // Act
            var result = _controller.Search(postcode: "12345");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var addresses = Assert.IsAssignableFrom<IEnumerable<AddressSearchResult>>(okResult.Value);
            var addressList = addresses.ToList();

            Assert.Single(addressList);
            Assert.Equal("123 Test St", addressList[0].Address);
            Assert.Equal("12345", addressList[0].PostCode);
            Assert.Equal("TestTown", addressList[0].Town);
            Assert.Equal("TestCounty", addressList[0].County);
            Assert.Equal("TestCountry", addressList[0].CountryName);
        }

        [Fact]
        public void Search_WithNonExistentPostcode_ReturnsEmptyResult()
        {
            // Act
            var result = _controller.Search(postcode: "99999");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var addresses = Assert.IsAssignableFrom<IEnumerable<AddressSearchResult>>(okResult.Value);
            Assert.Empty(addresses);
        }

        [Fact]
        public void Search_WithValidStreet_ReturnsCorrectResult()
        {
            // Act
            var result = _controller.Search(street: "Test St");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var addresses = Assert.IsAssignableFrom<IEnumerable<AddressSearchResult>>(okResult.Value);
            var addressList = addresses.ToList();

            Assert.Single(addressList);
            Assert.Equal("123 Test St", addressList[0].Address);
        }

        [Fact]
        public void Search_WithValidTown_ReturnsCorrectResult()
        {
            // Act
            var result = _controller.Search(town: "TestTown");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var addresses = Assert.IsAssignableFrom<IEnumerable<AddressSearchResult>>(okResult.Value);
            var addressList = addresses.ToList();

            Assert.Single(addressList);
            Assert.Equal("TestTown", addressList[0].Town);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Database.EnsureDeleted();
                    _context.Dispose();
                }

                _disposed = true;
            }
        }
    }