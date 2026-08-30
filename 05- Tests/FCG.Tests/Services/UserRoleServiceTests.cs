using FCG.Application.Services.Interfaces;
using FCG.Domain.Commons.Result;
using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using Moq;

namespace FCG.Tests.Services;

public class UserRoleServiceTests
{
    [Fact]
    public async Task GetAllAsync_Should_Return_Roles_From_Repository()
    {
        var repository = new Mock<IUserRoleRepository>();
        repository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<UserRole>
            {
                new() { Id = 1, Name = "User" },
                new() { Id = 2, Name = "Administrator" }
            });

        var service = new UserRoleService(repository.Object);
        var result = await service.GetAllAsync(TestContext.Current.CancellationToken);

        Assert.True(result.Success);
        Assert.Equal(2, result.Value!.Count);
        Assert.Equal("User", result.Value[0].Name);
    }
}
