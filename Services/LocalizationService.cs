using Microsoft.AspNetCore.Http;

namespace U.Services;

using System.Text.Json;

public interface ILocalizationService {
	string GetMessage(string key, string? locale = null);
}

public class LocalizationService(IHttpContextAccessor httpContext) : ILocalizationService {
	private readonly Dictionary<string, Dictionary<string, string>> _localizedMessages = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText("LocalizedMessages.json")) ?? new Dictionary<string, Dictionary<string, string>>();

	public string GetMessage(string key, string? locale) {
		return _localizedMessages.TryGetValue(locale ?? httpContext.HttpContext?.Request.Headers["Locale"].FirstOrDefault() ?? "en", out Dictionary<string, string>? messages) && messages.TryGetValue(key, out string? message)
			? message
			: _localizedMessages.GetValueOrDefault("en")?.GetValueOrDefault(key, "Message not found") ?? "Message not found";
	}
}