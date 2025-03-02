using FluentValidation;
using U.Constants;
using U.Services;
using U.Utils;

namespace U.Data.Params.UserManagement;

public class RegisterParams {
	public required string UserName { get; set; }
	public required string Email { get; set; }
	public required string PhoneNumber { get; set; }
	public required string Password { get; set; }
	public required List<TagUser> Tags { get; set; }
}

public class RegisterParamsValidator : AbstractValidator<RegisterParams> {
	public RegisterParamsValidator(ILocalizationService l) {
		RuleFor(x => x.Email)
			.NotEmpty().WithMessage(l.Get("EmailRequired"))
			.EmailAddress().WithMessage(l.Get("EmailInvalid"));

		RuleFor(x => x.Password)
			.NotEmpty().WithMessage(l.Get("PasswordRequired"))
			.MinimumLength(6).WithMessage(l.Get("PasswordMinLength"))
			.MaximumLength(100).WithMessage(l.Get("PasswordMaxLength"));

		RuleFor(x => x.UserName)
			.Must(username => username.MinMaxLenght(4, 40))
			.WithMessage(l.Get("UserNameInvalid"));
	}
}