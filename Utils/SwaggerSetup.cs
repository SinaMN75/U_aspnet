using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace U.Utils;

public static class SwaggerSetup {
	public static void AddUSwagger(this WebApplicationBuilder builder) {
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(c => {
			c.UseInlineDefinitionsForEnums();
			c.OrderActionsBy(s => s.RelativePath);
			c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
				Description = "JWT Authorization header.\r\n\r\nExample: \"Bearer 12345abcdef\"",
				Name = "Authorization",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer"
			});
			c.AddSecurityDefinition("apiKey", new OpenApiSecurityScheme {
				Description = "API KEY",
				Name = "X-API-KEY",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.ApiKey
			});
			
			c.AddSecurityDefinition("locale", new OpenApiSecurityScheme {
				Description = "Locale",
				Name = "locale",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.Http
			});

			c.AddSecurityRequirement(new OpenApiSecurityRequirement {
				{ new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() },
				{ new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "apiKey" } }, Array.Empty<string>() },
				{ new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.Header, Id = "locale" } }, Array.Empty<string>() }
			});
		});
	}
	
	public static void UseUSwagger(this IApplicationBuilder app) {
		app.UseSwagger();
		app.UseSwaggerUI(c => {
			c.DocExpansion(DocExpansion.None);
			c.DefaultModelsExpandDepth(2);
		});
	}
}