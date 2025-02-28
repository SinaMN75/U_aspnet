using U.Constants;

namespace U.Data.Params.UserManagement.Category;

public class CategoryUpdateParams {
	public required Guid Id { get; set; }
	public string? Title { get; set; }
	public string? TitleTr1 { get; set; }
	public string? TitleTr2 { get; set; }
	public string? Subtitle { get; set; }
	public List<TagCategory>? AddTags { get; set; }
	public List<TagCategory>? RemoveTags { get; set; }
}