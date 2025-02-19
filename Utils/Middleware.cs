namespace U.Utils;

public class ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration) {
	public async Task Invoke(HttpContext context) {
		if ((context.Request.Path.Value ?? "").Contains("api"))
			if (!context.Request.Headers.TryGetValue("X-API-Key", out Microsoft.Extensions.Primitives.StringValues apiKey) || apiKey != configuration.GetValue<string>("ApiKey")!) {
				context.Response.StatusCode = StatusCodes.Status404NotFound;
				return;
			}

		await next(context);
	}
}