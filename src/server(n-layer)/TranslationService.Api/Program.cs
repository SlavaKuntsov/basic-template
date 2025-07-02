using Common.Common;
using Common.Exceptions;
using Common.Swagger;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;
using TranslationService.Application.Extensions;
using TranslationService.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Host.UseSerilog(
	(context, config) =>
		config.ReadFrom.Configuration(context.Configuration).Enrich.FromLogContext());

services
	.AddCommon()
	.AddExceptions()
	.AddSwagger();

services
	.AddApplication()
	.AddPersistence(configuration);

var app = builder.Build();

app.ApplyMigrations();

app.UseExceptionHandler();

app.UseSwagger();

app.UseSwaggerUI(
	c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API v1");
	});

app.UseForwardedHeaders(
	new ForwardedHeadersOptions
	{
		ForwardedHeaders = ForwardedHeaders.All
	});
app.UseCors();

app.MapControllers();

app.Run();