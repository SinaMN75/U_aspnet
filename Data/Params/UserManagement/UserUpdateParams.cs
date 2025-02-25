using U.Constants;

namespace U.Data.Params.UserManagement;

public class UserUpdateParams : BaseParam {
	public required Guid Id { get; set; }
	public string? Password { get; set; }
	public string? RefreshToken { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Subtitle { get; set; }
	public string? Country { get; set; }
	public string? State { get; set; }
	public string? City { get; set; }
	public string? UserName { get; set; }
	public string? PhoneNumber { get; set; }
	public string? Email { get; set; }
	public string? FullName { get; set; }
	public string? Title { get; set; }
	public string? Bio { get; set; }
	public string? Instagram { get; set; }
	public string? Telegram { get; set; }
	public string? WhatsApp { get; set; }
	public string? LinkedIn { get; set; }
	public string? Website { get; set; }
	public string? Color { get; set; }
	public string? Code { get; set; }
	public string? Address { get; set; }
	public string? FatherName { get; set; }
	public string? NationalCode { get; set; }
	public string? PostalCode { get; set; }
	public string? LandlinePhone { get; set; }
	public string? SchoolName { get; set; }
	public string? Height { get; set; }
	public string? Weight { get; set; }
	public string? FoodAllergies { get; set; }
	public string? Sickness { get; set; }
	public string? UsedDrugs { get; set; }
	public string? FcmToken { get; set; }
	public string? PassportNumber { get; set; }
	public DateTime? Birthdate { get; set; }
	public List<TagUser>? AddTags { get; set; }
	public List<TagUser>? RemoveTags { get; set; }
}