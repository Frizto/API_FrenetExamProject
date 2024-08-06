using ApplicationLayer.CQRS.Address.Commands;
using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.User.Commands;
using ApplicationLayer.DTOs;
using ApplicationLayer.Extensions;
using DomainLayer.Enums;
using InfrastructureLayer.DataAccess;
using InfrastructureLayer.Handlers.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System.Security.Claims;

namespace xUnitTestLayer.Handlers;
public class UserHandlerTests
{
    private readonly DbContextOptions<AppDbContext> _options;
    private readonly Mock<UserManager<AppUser>> _mockUserManager;
    private readonly AppDbContext _dbContext;
    private readonly ICommandHandler<CreateUserCommand, ServiceResponse> _handler;

    public UserHandlerTests()
    {
        var _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _dbContext = new AppDbContext(_options);
        _mockUserManager = new Mock<UserManager<AppUser>>(new Mock<IUserStore<AppUser>>().Object, null, null, null, null, null, null, null, null);
        _handler = new CreateUserHandler(_mockUserManager.Object, _dbContext);
    }

    [Fact]
    public async Task TransactionalMethod_CanCommitChanges()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "test@example.com",
            Password = "Password123!",
            Name = "Test User",
            UserRole = AppUserTypeEnum.Client,
            Address = new CreateAddressCommand
            {
                Street = "123 Test St",
                City = "Test City",
                State = "TS",
                ZipCode = "12345"
            },
            Phone = "123-456-7890"
        };

        // Mock UserManager
        _mockUserManager.Setup(u => u.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(u => u.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(u => u.AddClaimsAsync(It.IsAny<AppUser>(), It.IsAny<IEnumerable<Claim>>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var service = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(service.Flag);
        Assert.True(service.DateTime < DateTime.UtcNow);
    }
    
    // Add more tests here as needed
}

