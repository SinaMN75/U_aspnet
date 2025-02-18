namespace U.Utils;

public static class StringExtension {
	public static bool IsNotNullOrEmpty(this string? s) => s is { Length: > 0 };

	public static bool IsNotNull(this string? s) => s != null;

	public static bool IsNullOrEmpty(this string? s) => string.IsNullOrEmpty(s);

	public static bool IsNull(this string? s) => s == null;
}

public static class Core {
	public static readonly JsonSerializerOptions? JsonSettings = new() {
		WriteIndented = true,
		ReferenceHandler = ReferenceHandler.IgnoreCycles,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
	};
}

public static class EnumExtension {
	public static IEnumerable<IdTitleDto> GetValues<T>() => (from int itemType in Enum.GetValues(typeof(T))
		select new IdTitleDto { Title = Enum.GetName(typeof(T), itemType), Id = itemType }).ToList();
}

public static class UtilitiesStatusCodesExtension {
	public static int Value(this UtilitiesStatusCodes statusCode) => (int)statusCode;
}

public static class EnumerableExtension {
	public static bool IsNotNullOrEmpty<T>(this IEnumerable<T>? list) => list != null && list.Any();

	public static bool IsNotNull<T>(this IEnumerable<T>? list) => list != null;

	public static bool IsNullOrEmpty<T>(this IEnumerable<T>? list) => list == null || list.Any();

	public static async Task<GenericResponse<IQueryable<T>>> Paginate<T>(this IQueryable<T> q, BaseFilterDto dto) where T : BaseEntity {
		int totalCount = await q.CountAsync();
		q = q.Skip((dto.PageNumber - 1) * dto.PageSize).Take(dto.PageSize).AsNoTracking();

		return new GenericResponse<IQueryable<T>>(q) {
			TotalCount = totalCount,
			PageCount = totalCount % dto.PageSize == 0 ? totalCount / dto.PageSize : totalCount / dto.PageSize + 1,
			PageSize = dto.PageSize
		};
	}
}