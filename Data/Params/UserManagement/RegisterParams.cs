using U.Constants;

namespace U.Data.Params.UserManagement;

public class RegisterParams {
	public required string UserName { get; set; }
	public required string Email { get; set; }
	public required string PhoneNumber { get; set; }
	public required string Password { get; set; }
	public required List<TagUser> Tags { get; set; }
}