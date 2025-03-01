using Microsoft.AspNetCore.Http;
using System.Text;
using U.Utils;

namespace U.Middlewares;

public class Base64EncodeMiddleware(RequestDelegate next) {
	public async Task InvokeAsync(HttpContext context) {
		Stream originalBodyStream = context.Response.Body;
		using MemoryStream newBodyStream = new();
		context.Response.Body = newBodyStream;
		await next(context);
		newBodyStream.Seek(0, SeekOrigin.Begin);
		string responseBody = await new StreamReader(newBodyStream).ReadToEndAsync();
		string base64ResponseBody = Encryption.EncodeBase64(responseBody);
		newBodyStream.Seek(0, SeekOrigin.Begin);
		newBodyStream.SetLength(0);
		await newBodyStream.WriteAsync(Encoding.UTF8.GetBytes(base64ResponseBody));
		newBodyStream.Seek(0, SeekOrigin.Begin);
		await newBodyStream.CopyToAsync(originalBodyStream);
	}
}

public class Base64DecodeMiddleware(RequestDelegate next) {
	public async Task InvokeAsync(HttpContext context) {
		if (context.Request.ContentLength > 0) {
			using StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8);
			string base64Body = await reader.ReadToEndAsync();

			try {
				string decodedBody = Encryption.DecodeBase64(base64Body);
				MemoryStream newBodyStream = new(Encoding.UTF8.GetBytes(decodedBody));
				context.Request.Body = newBodyStream;
				context.Request.ContentLength = newBodyStream.Length;

				newBodyStream.Seek(0, SeekOrigin.Begin);
			}
			catch (FormatException) {
				context.Response.StatusCode = StatusCodes.Status400BadRequest;
				await context.Response.WriteAsync("Invalid Base64 input.");
				return;
			}
		}

		await next(context);
	}
}