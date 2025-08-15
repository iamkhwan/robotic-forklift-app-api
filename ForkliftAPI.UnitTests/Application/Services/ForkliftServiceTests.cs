using Moq;
using Xunit;
using ForkliftAPI.Application.Services;
using ForkliftAPI.Domain.Entities;
using ForkliftAPI.Application.Interfaces;
using AutoMapper;
using ForkliftAPI.Application.DTOs;

namespace ForkliftAPI.UnitTests.Services
{
    public class ForkliftServiceTests
    {
        [Fact]
        public async Task GetForklifts_ShouldReturnAllForklifts()
        {
            // Arrange
            var mockRepo = new Mock<IForkliftRepository>();
            var mockMapper = new Mock<IMapper>();

            var forkliftDtos = new List<ForkliftDto>
            {
                new ForkliftDto { Name = "Forklift 1", ModelNumber = "M1", ManufacturingDate = "2025-01-01" },
                new ForkliftDto { Name = "Forklift 2", ModelNumber = "M2", ManufacturingDate = "2025-01-02" }
            };

            var forklifts = new List<Forklift>
            {
                new Forklift("Forklift 1", "M1", "2025-01-01"),
                new Forklift("Forklift 2", "M2", "2025-01-02")
            };

            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(forklifts);

            mockMapper.Setup(m => m.Map<IEnumerable<ForkliftDto>>(It.IsAny<IEnumerable<Forklift>>()))
                        .Returns(forkliftDtos);

            var service = new ForkliftService(mockRepo.Object, mockMapper.Object);

            // Act
            var result = await service.GetAllForkliftsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetForklifts_ShouldReturnEmptyList()
        {
            // Arrange
            var mockRepo = new Mock<IForkliftRepository>();
            var mockMapper = new Mock<IMapper>();

            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Forklift>());

            var service = new ForkliftService(mockRepo.Object, mockMapper.Object);

            // Act
            var result = await service.GetAllForkliftsAsync();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetForklifts_RepositoryThrows_ExceptionPropagates()
        {
            // Arrange
            var mockRepo = new Mock<IForkliftRepository>();
            var mockMapper = new Mock<AutoMapper.IMapper>();

            // Make the repository throw an exception
            mockRepo.Setup(r => r.GetAllAsync())
                    .ThrowsAsync(new Exception("Repository failure"));

            var service = new ForkliftService(
                mockRepo.Object,
                mockMapper.Object
            );

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(
                () => service.GetAllForkliftsAsync()
            );

            // Assert
            Assert.Equal("Repository failure", exception.Message);
        }

        [Fact]
        public async Task ClearForkLift_CallsRepositoryOnce()
        {
            // Arrange
            var mockRepo = new Mock<IForkliftRepository>();
            var mockMapper = new Mock<IMapper>();

            var service = new ForkliftService(mockRepo.Object, mockMapper.Object);

            // Act
            await service.ClearAllForkliftsAsync();

            // Assert
            mockRepo.Verify(repo => repo.ClearAllForkliftsAsync(), Times.Once);
        }

        [Fact]
        public async Task ClearForkLift_RepositoryThrows_ExceptionPropagates()
        {
            // Arrange
            var mockRepo = new Mock<IForkliftRepository>();
            var mockMapper = new Mock<IMapper>();

            // Simulate repository throwing an exception
            mockRepo.Setup(r => r.ClearAllForkliftsAsync())
                    .ThrowsAsync(new Exception("Database error"));

            var service = new ForkliftService(
                mockRepo.Object,
                mockMapper.Object
            );

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(
                () => service.ClearAllForkliftsAsync()
            );

            // Assert
            Assert.Equal("Database error", exception.Message);
        }

        [Fact]
        public async Task UploadForkliftsAsync_AddsOnlyNewForklifts_ReturnsCorrectCount()
        {
            // Arrange
            var mockRepo = new Mock<IForkliftRepository>();
            var mockMapper = new Mock<IMapper>();

            var forkliftDtos = new List<ForkliftDto>
            {
                new ForkliftDto { Name = "Forklift 1", ModelNumber = "M1", ManufacturingDate = "2025-01-01" },
                new ForkliftDto { Name = "Forklift 2", ModelNumber = "M2", ManufacturingDate = "2025-01-02" }
            };

            var forklifts = new List<Forklift>
            {
                new Forklift("Forklift A", "M1", "2025-01-01"),
                new Forklift("Forklift B", "M2", "2025-01-02")
            };

            // Mock mapping behavior
            mockMapper.Setup(m => m.Map<List<Forklift>>((forkliftDtos)))
                    .Returns(forklifts);

            // Mock repository: only "M1" already exists
            mockRepo.Setup(r => r.GetExistingModelNumbersAsync(It.IsAny<List<string>>()))
                    .ReturnsAsync(new List<string> { "M1" });

            // Mock AddRangeAsync to just return completed task
            mockRepo.Setup(r => r.AddRangeAsync(It.IsAny<List<Forklift>>()))
                    .Returns(Task.CompletedTask);

            var service = new ForkliftService(mockRepo.Object, mockMapper.Object);

            // Act
            var result = await service.UploadForkliftsAsync(forkliftDtos);

            // Assert
            Assert.Equal(1, result); // Only one new forklift should be added

            mockRepo.Verify(r => r.AddRangeAsync(
                It.Is<List<Forklift>>(f => f.Count == 1 && f[0].ModelNumber == "M2")
            ), Times.Once);
        }

        [Fact]
        public async Task UploadForkliftsAsync_Returns0_IfAllExist()
        {
            // Arrange
            var mockRepo = new Mock<IForkliftRepository>();
            var mockMapper = new Mock<IMapper>();

            var forkliftsDto = new List<ForkliftDto>
            {
                new ForkliftDto { Name = "Forklift 1", ModelNumber = "M1", ManufacturingDate = "2025-01-01" },
                new ForkliftDto { Name = "Forklift 2", ModelNumber = "M2", ManufacturingDate = "2025-01-02" }
            };

            var forklifts = new List<Forklift>
            {
                new Forklift("Forklift A", "M1", "2025-01-01"),
                new Forklift("Forklift B", "M2", "2025-01-02")
            };

            // Mock mapping behavior
            mockMapper.Setup(m => m.Map<List<Forklift>>((forkliftsDto)))
                    .Returns(forklifts);

            // Mock repository
            mockRepo.Setup(r => r.AddRangeAsync(forklifts))
                    .Returns(Task.CompletedTask);

            var service = new ForkliftService(mockRepo.Object, mockMapper.Object);

            // Act
            var result = await service.UploadForkliftsAsync(forkliftsDto);

            // Assert
            Assert.Equal(0, result);
            mockRepo.Verify(r => r.AddRangeAsync(It.IsAny<IEnumerable<Forklift>>()), Times.Never);
        }

        [Fact]
        public async Task UploadForkliftsAsync_Adds_Only_New_Items_When_Mixed()
        {
            // Arrange
            var mockRepo = new Mock<IForkliftRepository>();
            var mockMapper = new Mock<IMapper>();

            // Incoming DTOs (two existing, one new)
            var incomingDtos = new List<ForkliftDto>
            {
                new ForkliftDto { Name = "Forklift A", ModelNumber = "M1", ManufacturingDate = "2025-01-01" },
                new ForkliftDto { Name = "Forklift B", ModelNumber = "M2", ManufacturingDate = "2025-01-02" },
                new ForkliftDto { Name = "Forklift C", ModelNumber = "M3", ManufacturingDate = "2025-01-03" } // new one
            };

            mockMapper.Setup(m => m.Map<List<Forklift>>(incomingDtos))
                    .Returns(new List<Forklift>
                    {
                        new Forklift("Forklift A", "M1", "2025-01-01"),
                        new Forklift("Forklift B", "M2", "2025-01-02"),
                        new Forklift("Forklift C", "M3", "2025-01-03") // new one
                    });

            // Existing forklifts (by model number)
            var existingModelNumbers = new List<string> { "M1", "M2" };
            mockRepo.Setup(r => r.GetExistingModelNumbersAsync(It.IsAny<List<string>>()))
                    .ReturnsAsync(existingModelNumbers);

            mockRepo.Setup(r => r.AddRangeAsync(It.IsAny<IEnumerable<Forklift>>()))
                    .Returns(Task.CompletedTask);

            var service = new ForkliftService(mockRepo.Object, mockMapper.Object);

            // Act
            var result = await service.UploadForkliftsAsync(incomingDtos);

            // Assert
            Assert.Equal(1, result); // Only 1 should be added
            mockRepo.Verify(r => r.AddRangeAsync(
                It.Is<IEnumerable<Forklift>>(f => f.Count() == 1 && f.First().Name == "Forklift C")
            ), Times.Once);
        }

    }
}