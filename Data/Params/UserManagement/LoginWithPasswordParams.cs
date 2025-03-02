using System.ComponentModel.DataAnnotations;

namespace U.Data.Params.UserManagement;

public class LoginWithEmailPasswordParams: BaseParam {
	[Required]
	[MinLength(5)]
	[MaxLength(100)]
	[EmailAddress]
	public required string Email { get; set; }

	[Required]
	[MinLength(6)]
	[MaxLength(100)]
	public required string Password { get; set; }
}