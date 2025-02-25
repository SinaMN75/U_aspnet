using System.ComponentModel.DataAnnotations;

namespace U.Data.Params.UserManagement;

public class RefreshTokenParams {
	[Required]
	public required string RefreshToken { get; set; }
}