using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace U.Utils;

public static class StringExtension {
	public static bool IsNotNullOrEmpty(this string? s) => s is { Length: > 0 };

	public static bool IsNotNull(this string? s) => s != null;

	public static bool IsNullOrEmpty(this string? s) => string.IsNullOrEmpty(s);

	public static bool IsNull(this string? s) => s == null;

	public static bool IsEmail(this string email) => !string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
}

public static class Core {
	public static readonly JsonSerializerOptions? JsonSettings = new() {
		WriteIndented = true,
		ReferenceHandler = ReferenceHandler.IgnoreCycles,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
	};
}

public static class EnumExtension {
	public static IEnumerable<IdTitleDto> GetValues<T>() =>
		(from int itemType in Enum.GetValues(typeof(T))
			select new IdTitleDto { Title = Enum.GetName(typeof(T), itemType), Id = itemType }).ToList();
}

public static class UtilitiesStatusCodesExtension {
	public static int Value(this UStatusCodes statusCode) => (int)statusCode;
}

public static class EnumerableExtension {
	public static bool IsNotNullOrEmpty<T>(this IEnumerable<T>? list) => list != null && list.Any();

	public static bool IsNotNull<T>(this IEnumerable<T>? list) => list != null;

	public static bool IsNullOrEmpty<T>(this IEnumerable<T>? list) => list == null || list.Any();
}

public static class GenericResponseExtensions {
	public static IResult ToResult(this GenericResponse response) => TypedResults.Json(response, statusCode: response.Status.Value());
}