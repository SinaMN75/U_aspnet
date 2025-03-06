using Microsoft.AspNetCore.Http;
using U.Constants;

namespace U.Data.Params.Media;

public class MediaCreateParams: BaseParam {
	public required IFormFile File { get; set; }
	public required List<TagMedia> Tags { get; set; }
	public Guid? UserId { get; set; }
	public string? Title { get; set; }
	public string? Description { get; set; }
}