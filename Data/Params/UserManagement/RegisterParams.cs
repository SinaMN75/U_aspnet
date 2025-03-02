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
	public RegisterParamsValidator(ILocalizationService localeService) {
		RuleFor(x => x.Email)
			.Must(email => email.IsEmail())
			.WithMessage(localeService.Get("EmailInvalid"));

		RuleFor(x => x.Password)
			.Must(password => password.MinMaxLenght(6, 100))
			.WithMessage(localeService.Get("PasswordInvalid"));

		RuleFor(x => x.UserName)
			.Must(username => username.MinMaxLenght(4, 40))
			.WithMessage(localeService.Get("UserNameInvalid"));
	}
}