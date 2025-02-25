using Microsoft.AspNetCore.Http;

namespace U.Services;

using System.Text.Json;

public interface ILocalizationService {
	string Get(string key, string? locale = null);
}

public class LocalizationService(IHttpContextAccessor httpContext) : ILocalizationService {
	public string Get(string key, string? locale) => JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText("LocalizedMessages.json"))!.GetValueOrDefault(locale ?? httpContext.HttpContext?.Request.Headers["Locale"].FirstOrDefault() ?? "en")?.GetValueOrDefault(key, "Error") ?? "Error";
}