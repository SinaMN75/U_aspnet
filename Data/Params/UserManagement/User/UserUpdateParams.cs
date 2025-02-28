using U.Constants;

namespace U.Data.Params.UserManagement.User;

public class UserUpdateParams : BaseParam {
	public required Guid Id { get; set; }
	public string? Password { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Country { get; set; }
	public string? State { get; set; }
	public string? City { get; set; }
	public string? UserName { get; set; }
	public string? PhoneNumber { get; set; }
	public string? Email { get; set; }
	public string? FullName { get; set; }
	public string? Bio { get; set; }
	public string? FcmToken { get; set; }
	public DateTime? Birthdate { get; set; }
	public List<TagUser>? AddTags { get; set; }
	public List<TagUser>? RemoveTags { get; set; }
}