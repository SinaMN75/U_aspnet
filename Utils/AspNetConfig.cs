namespace U.Utils;

public static class AspNetConfig {
	public static void AddUServices<T>(this WebApplicationBuilder builder) where T : DbContext {
		builder.WebHost.ConfigureKestrel(o => o.ConfigureEndpointDefaults(e => e.Protocols = HttpProtocols.Http2));
		builder.Services.AddCors(c => c.AddPolicy("AllowOrigin", option => option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
		builder.Services.AddOpenApi();
		builder.AddUSwagger();
		builder.AddUOutputCache();
		builder.Services.AddHttpContextAccessor();
		Server.Configure(builder.Services.BuildServiceProvider().GetService<IServiceProvider>()?.GetService<IHttpContextAccessor>());
		builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
		builder.Services.ConfigureHttpJsonOptions(o => {
			o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
			o.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
			o.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
			o.SerializerOptions.WriteIndented = false;
		});
		builder.Services.AddRateLimiter(o => o.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
			RateLimitPartition.GetFixedWindowLimiter(
				context.Request.Headers.Host.ToString(),
				_ => new FixedWindowRateLimiterOptions {
					AutoReplenishment = true,
					PermitLimit = 100,
					Window = TimeSpan.FromMinutes(1)
				})));

		builder.Services.AddResponseCompression(o => o.EnableForHttps = true);
		builder.Services.AddScoped<DbContext, T>();
		builder.Services.AddDbContextPool<T>(b => b.UseNpgsql(builder.Configuration.GetConnectionString("ServerPostgres"), o => {
			AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
			o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
			o.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
		}));

		builder.Services.AddHttpClient<HttpClientService>();
		builder.Services.AddMemoryCache();
		builder.Services.AddScoped<HttpClientService>();
		builder.Services.AddSingleton<ILocalizationService, LocalizationService>();
		builder.Services.AddSingleton<IJwtService, JwtService>();
	}

	public static void UseUServices(this WebApplication app) {
		app.UseCors(o => o.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
		app.UseResponseCompression();
		app.UseDeveloperExceptionPage();
		app.MapOpenApi();
		app.MapScalarApiReference();
		app.UseUSwagger();
		app.UseHttpsRedirection();
		app.UseRateLimiter();
		// app.UseMiddleware<ApiKeyMiddleware>();
	}
}