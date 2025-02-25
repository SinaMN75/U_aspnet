using System.ComponentModel.DataAnnotations;

namespace U;

public class BaseEntity {
	[Key]
	public required Guid Id { get; set; }

	public required DateTime CreatedAt { get; set; }
	public required DateTime UpdatedAt { get; set; }
}

public class IdDto {
	public required Guid Id { get; set; }
}

public class IdTitleDto {
	public int? Id { get; set; }
	public string? Title { get; set; }
}

public class BaseFilterDto : BaseParam {
	public int PageSize { get; set; } = 100;
	public int PageNumber { get; set; } = 1;
	public DateTime? FromDate { get; set; }
}

public class BaseParam {
	[Required]
	public required string ApiKey { get; set; }

	public string? Token { get; set; }
}

public class GenericResponse<T> : GenericResponse {
	public GenericResponse(T result, UStatusCodes status = UStatusCodes.Success, string message = "") {
		Result = result;
		Status = status;
		Message = message;
	}

	public T? Result { get; }
}

public class GenericResponse(UStatusCodes status = UStatusCodes.Success, string message = "") {
	public UStatusCodes Status { get; protected set; } = status;
	public int? PageSize { get; set; }
	public int? PageCount { get; set; }
	public int? TotalCount { get; set; }
	public string Message { get; set; } = message;
}