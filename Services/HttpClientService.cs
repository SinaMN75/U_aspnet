namespace U.Services;

public interface IHttpClientRepository {
	Task<string> Get(
		string uri,
		string? customCacheKey = null,
		TimeSpan? cacheDuration = null,
		Action<HttpRequestHeaders>? configureHeaders = null);

	Task<string> Post(
		string uri,
		object? body,
		string? customCacheKey = null,
		TimeSpan? cacheDuration = null,
		Action<HttpRequestHeaders>? configureHeaders = null);

	Task<string> Put(
		string uri,
		object? body,
		string? customCacheKey = null,
		TimeSpan? cacheDuration = null,
		Action<HttpRequestHeaders>? configureHeaders = null);

	Task<string> Delete(
		string uri,
		string? customCacheKey = null,
		TimeSpan? cacheDuration = null,
		Action<HttpRequestHeaders>? configureHeaders = null);
}

public class HttpClientRepository(HttpClient httpClient, IMemoryCache cache) : IHttpClientRepository {
	private readonly IMemoryCache _cache = cache ?? throw new ArgumentNullException(nameof(cache));
	private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

	public async Task<string> Get(
		string uri,
		string? customCacheKey = null,
		TimeSpan? cacheDuration = null,
		Action<HttpRequestHeaders>? configureHeaders = null) {
		return await Send(HttpMethod.Get, uri, null, customCacheKey, cacheDuration, configureHeaders);
	}

	public async Task<string> Post(
		string uri,
		object? body,
		string? customCacheKey = null,
		TimeSpan? cacheDuration = null,
		Action<HttpRequestHeaders>? configureHeaders = null) {
		return await Send(HttpMethod.Post, uri, body, customCacheKey, cacheDuration, configureHeaders);
	}

	public async Task<string> Put(
		string uri,
		object? body,
		string? customCacheKey = null,
		TimeSpan? cacheDuration = null,
		Action<HttpRequestHeaders>? configureHeaders = null) {
		return await Send(HttpMethod.Put, uri, body, customCacheKey, cacheDuration, configureHeaders);
	}

	public async Task<string> Delete(
		string uri,
		string? customCacheKey = null,
		TimeSpan? cacheDuration = null,
		Action<HttpRequestHeaders>? configureHeaders = null) {
		return await Send(HttpMethod.Delete, uri, null, customCacheKey, cacheDuration, configureHeaders);
	}

	private async Task<string> Send(
		HttpMethod method,
		string uri,
		object? body = null,
		string? customCacheKey = null,
		TimeSpan? cacheDuration = null,
		Action<HttpRequestHeaders>? configureHeaders = null) {
		if (string.IsNullOrEmpty(uri)) throw new ArgumentException("URI cannot be null or empty.", nameof(uri));

		string? cacheKey = cacheDuration.HasValue ? customCacheKey ?? $"{method}-{uri}" : null;
		if (cacheKey != null && _cache.TryGetValue(cacheKey, out string? cachedResponse) && cachedResponse != null) {
			return cachedResponse;
		}

		using HttpRequestMessage request = new(method, uri);
		if (body != null) {
			string json = JsonSerializer.Serialize(body);
			request.Content = new StringContent(json, Encoding.UTF8, "application/json");
		}

		configureHeaders?.Invoke(request.Headers);
		using HttpResponseMessage response = await _httpClient.SendAsync(request);
		if (!response.IsSuccessStatusCode) {
			throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
		}

		string responseContent = await response.Content.ReadAsStringAsync();
		if (!cacheDuration.HasValue || cacheKey == null) return responseContent;

		MemoryCacheEntryOptions cacheOptions = new() {
			AbsoluteExpirationRelativeToNow = cacheDuration.Value
		};
		_cache.Set(cacheKey, responseContent, cacheOptions);

		return responseContent;
	}
}