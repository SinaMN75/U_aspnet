namespace U.Utils;

public static class AspNetConfig {
	public static void UseUtilitiesServices(this WebApplication app, bool log = false) {
		app.UseCors(option => option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
		app.UseMiddleware<ApiKeyMiddleware>();
		app.UseRateLimiter();
		app.UseOutputCache();
		app.UseDeveloperExceptionPage();
		app.Use(async (context, next) => {
			await next();
			if (context.Response.StatusCode == 401)
				await context.Response.WriteAsJsonAsync(new GenericResponse(UtilitiesStatusCodes.UnAuthorized));
		});
	}
}