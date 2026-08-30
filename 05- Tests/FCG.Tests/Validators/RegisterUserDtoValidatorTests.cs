using FCG.Application.DTOs.Users;
using FCG.Application.Validators;
using FluentValidation.TestHelper;

namespace FCG.Tests.Validators;

public class RegisterUserDtoValidatorTests
{
    private readonly RegisterUserDtoValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var dto = new RegisterUserDto { Name = "", Email = "user@test.com", Password = "Abcdef1!" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var dto = new RegisterUserDto { Name = "User", Email = "invalid", Password = "Abcdef1!" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Weak()
    {
        var dto = new RegisterUserDto { Name = "User", Email = "user@test.com", Password = "weak" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Dto_Is_Valid()
    {
        var dto = new RegisterUserDto { Name = "User", Email = "user@test.com", Password = "Abcdef1!" };
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
