namespace U.Services;

using System.Text.Json;

public interface ILocalizationService {
	string GetMessage(string locale, string key);
}

public class LocalizationService : ILocalizationService {
	private readonly Dictionary<string, Dictionary<string, string>> _localizedMessages;

	public LocalizationService() {
		try {
			string json = File.ReadAllText("LocalizedMessages.json");
			_localizedMessages = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json) ?? new Dictionary<string, Dictionary<string, string>>();
		}
		catch (Exception ex) {
			Console.WriteLine($"Error loading localization file: {ex.Message}");
			_localizedMessages = new Dictionary<string, Dictionary<string, string>>();
		}
	}

	public string GetMessage(string locale, string key) {
		return _localizedMessages.TryGetValue(locale, out Dictionary<string, string>? messages) && messages.TryGetValue(key, out string? message)
			? message
			: _localizedMessages.GetValueOrDefault("en")?.GetValueOrDefault(key, "Message not found") ?? "Message not found";
	}
}