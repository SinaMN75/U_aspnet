using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.Text.Json;

namespace U.Services;

public interface ILocalizationService {
	string Get(string key, string? locale = null);
}

public class LocalizationService(IHttpContextAccessor httpContext) : ILocalizationService {
	public string Get(string key, string? locale) {
		return JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(ReadJsonFile())!.GetValueOrDefault(locale ?? httpContext.HttpContext?.Request.Headers["Locale"].FirstOrDefault() ?? "en")?.GetValueOrDefault(key, "Error") ?? "Error";
	}

	private string ReadJsonFile() {
		Assembly assembly = Assembly.GetExecutingAssembly();
		const string resourceName = "U_aspnet.LocalizedMessages.json";

		using Stream? stream = assembly.GetManifestResourceStream(resourceName);
		using StreamReader reader = new(stream);
		return reader.ReadToEnd();
	}
}