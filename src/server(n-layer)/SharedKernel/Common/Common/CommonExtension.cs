using Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Common;

public static class CommonExtension
{
	public static IServiceCollection AddCommon(this IServiceCollection services)
	{
		services.AddProblemDetails();

		services.AddHttpContextAccessor();

		services.AddControllers();

		services.AddEndpointsApiExplorer();

		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

		return services;
	}
}