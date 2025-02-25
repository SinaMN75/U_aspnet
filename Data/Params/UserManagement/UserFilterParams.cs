using U.Constants;

namespace U.Data.Params.UserManagement;

public class UserFilterParams : BaseFilterDto {
	public string? UserName { get; set; }
	public string? PhoneNumber { get; set; }
	public string? Email { get; set; }
	public string? FullName { get; set; }
	public string? Title { get; set; }
	public string? Bio { get; set; }
	public DateTime? StartBirthDate { get; set; }
	public DateTime? EndBirthDate { get; set; }
	public List<TagUser>? Tags { get; set; }
}