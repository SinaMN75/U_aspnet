using U.Constants;

namespace U.Data.Params.UserManagement.Category;

public class CategoryCreateParams: BaseParam {
	public required string Title { get; set; }
	public string? TitleTr1 { get; set; }
	public string? TitleTr2 { get; set; }
	public string? Subtitle { get; set; }
	public required List<TagCategory> Tags { get; set; }
}