namespace U.Dto;

public class IdDto {
	public required Guid Id { get; set; }
}

public class BaseEntity {
	public required Guid Id { get; set; }
	public required DateTime CreatedAt { get; set; }
	public required DateTime UpdatedAt { get; set; }
}
