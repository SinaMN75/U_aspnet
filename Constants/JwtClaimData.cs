namespace U.Constants;

public class JwtClaimData {
	public string? Id { get; set; }
	public string? Email { get; set; }
	public string? PhoneNumber { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? FullName { get; set; }
	public string? Tags { get; set; }
	public DateTime? Expiration { get; set; }
	public bool IsExpired => Expiration.HasValue && Expiration.Value < DateTime.UtcNow;
	public List<TagUser> TagsList => Tags!.Split(',').Select(Enum.Parse<TagUser>).ToList();
}