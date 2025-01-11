namespace PetManager.Application.Users.Commands.SignIn;

internal class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .NotNull().WithMessage("Email cannot be null")
            .EmailAddress().WithMessage("Email must be a valid email address");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .NotNull().WithMessage("Password cannot be null")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
            .MaximumLength(16).WithMessage("Password must be at most 16 characters long");
    }
}