namespace U.Services;

public interface IJwtService {
	public string GenerateRefreshToken();
	public string GenerateJwt(IEnumerable<Claim> claims);
	public JwtClaimData? ExtractClaims(string token);
	public bool ValidateApiKey(string apiKey);
}

public class JwtService(IConfiguration config) : IJwtService {
	public string GenerateRefreshToken() {
		byte[] randomNumber = new byte[64];
		using RandomNumberGenerator rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		return Convert.ToBase64String(randomNumber);
	}

	public string GenerateJwt(IEnumerable<Claim> claims) {
		return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
				config["Jwt:Issuer"]!,
				config["Jwt:Audience"]!,
				claims,
				expires: DateTime.UtcNow.Add(TimeSpan.FromSeconds(60)),
				signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!)), SecurityAlgorithms.HmacSha256)
			)
		);
	}

	public JwtClaimData? ExtractClaims(string token) {
		try {
			IEnumerable<Claim> claims = new JwtSecurityTokenHandler().ReadJwtToken(token).Claims.ToList();
			return new JwtClaimData {
				Id = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value ?? "",
				Email = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value ?? "",
				PhoneNumber = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.PhoneNumber)?.Value ?? "",
				FirstName = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value ?? "",
				LastName = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.FamilyName)?.Value ?? "",
				FullName = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.GivenName)?.Value ?? "",
				Expiration = DateTime.Parse(claims.FirstOrDefault(c => c.Type == ClaimTypes.Expiration)?.Value ?? ""),
				Tags = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? ""
			};
		}
		catch (Exception e) {
			return null;
		}
	}

	public bool ValidateApiKey(string apiKey) {
		return apiKey == config["ApiKey"];
	}
}