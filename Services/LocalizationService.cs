namespace U.Services;

public interface ILocalizationService {
	string Get(string key, string? locale = null);
}

public class LocalizationService(IHttpContextAccessor httpContext) : ILocalizationService {
	public string Get(string key, string? locale) {
		string filePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "U_aspnet", "LocalizedMessages.json");
		filePath = Path.GetFullPath(filePath);
		return JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText(filePath))!.GetValueOrDefault(locale ?? httpContext.HttpContext?.Request.Headers["Locale"].FirstOrDefault() ?? "en")?.GetValueOrDefault(key, "Error") ?? "Error";
	}
}