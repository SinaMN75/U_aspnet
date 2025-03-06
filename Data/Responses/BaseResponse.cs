namespace U.Data.Responses;

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