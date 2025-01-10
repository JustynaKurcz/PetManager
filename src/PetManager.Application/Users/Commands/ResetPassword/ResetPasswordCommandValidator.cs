namespace PetManager.Application.Users.Commands.ResetPassword;

internal sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .NotNull().WithMessage("Email cannot be null")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Password cannot be empty")
            .NotNull().WithMessage("Password cannot be null")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .MaximumLength(16).WithMessage("Password must be at most 16 characters long");
    }
}