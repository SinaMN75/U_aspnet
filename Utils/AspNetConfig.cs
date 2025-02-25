using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scalar.AspNetCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

namespace U.Utils;

public static class AspNetConfig {
	public static void AddUConfigs<T>(this WebApplicationBuilder builder) where T : DbContext {
		builder.Logging.ClearProviders();
		builder.Logging.AddConsole();
		builder.Logging.SetMinimumLevel(LogLevel.Warning);
		builder.WebHost.ConfigureKestrel(o => o.ConfigureEndpointDefaults(e => e.Protocols = HttpProtocols.Http2));
		builder.Services.AddCors(c => c.AddPolicy("AllowOrigin", option => option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
		builder.Services.AddOpenApi();
		builder.AddUtilitiesSwagger();
		builder.Services.AddHttpContextAccessor();
		builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
		builder.Services.ConfigureHttpJsonOptions(options => {
			options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
			options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
			options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
			options.SerializerOptions.WriteIndented = false;
		});
		builder.Services.AddRateLimiter(options => options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
			RateLimitPartition.GetFixedWindowLimiter(
				context.Request.Headers.Host.ToString(),
				_ => new FixedWindowRateLimiterOptions {
					AutoReplenishment = true,
					PermitLimit = 100,
					Window = TimeSpan.FromMinutes(1)
				})));

		builder.Services.AddResponseCompression(o => o.EnableForHttps = true);
		builder.Services.AddDbContextPool<T>(b => b.UseNpgsql(builder.Configuration.GetConnectionString("ServerPostgres"), o => {
			AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
			o.EnableRetryOnFailure(2);
			o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
			o.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
		}));
	}


	public static void UseUtilitiesServices(this WebApplication app, bool log = false) {
		app.UseCors(o => o.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
		app.UseResponseCompression();
		app.UseDeveloperExceptionPage();
		app.MapOpenApi();
		app.MapScalarApiReference();
		app.UseUtilitiesSwagger();
		app.UseHttpsRedirection();
		app.UseRateLimiter();
	}
}