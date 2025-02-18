namespace U.Utils;

public static class StringExtension {
	public static bool IsNotNullOrEmpty(this string? s) => s is { Length: > 0 };

	public static bool IsNotNull(this string? s) => s != null;

	public static bool IsNullOrEmpty(this string? s) => string.IsNullOrEmpty(s);

	public static bool IsNull(this string? s) => s == null;
}