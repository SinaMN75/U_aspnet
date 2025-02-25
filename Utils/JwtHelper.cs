using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace U.Utils;

public class JwtHelper {
	public static JwtClaimData ExtractClaims(string token) {
		IEnumerable<Claim> claims = new JwtSecurityTokenHandler().ReadJwtToken(token).Claims.ToList();
		return new JwtClaimData {
			Id = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value ?? "",
			Email = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value ?? "",
			PhoneNumber = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.PhoneNumber)?.Value ?? "",
			FirstName = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value ?? "",
			LastName = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.FamilyName)?.Value ?? "",
			FullName = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.GivenName)?.Value ?? "",
			Expiration = DateTime.Parse(claims.FirstOrDefault(c => c.Type == ClaimTypes.Expiration)?.Value ?? ""),
			Tags = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "",
		};
	}

	public static string CreateRefresh() {
		byte[] randomNumber = new byte[64];
		using RandomNumberGenerator rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		return Convert.ToBase64String(randomNumber);
	}

	public string CreateJwt(
		string issuer,
		string audience,
		string key,
		TimeSpan expireTime,
		IEnumerable<Claim> claims
	) =>
		new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
			issuer,
			audience,
			claims,
			expires: DateTime.UtcNow.Add(expireTime),
			signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256)
		)
		);
}

public class JwtClaimData {
	public string? Id { get; set; }
	public string? Email { get; set; }
	public string? PhoneNumber { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? FullName { get; set; }
	public DateTime? Expiration { get; set; }
	public string? Tags { get; set; }
}