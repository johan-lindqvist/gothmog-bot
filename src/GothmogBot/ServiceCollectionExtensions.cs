using System.Net.Http.Headers;
using System.Reactive;

namespace GothmogBot;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddStratzGraphQLClient(this IServiceCollection services, string apiKey)
	{
		services
			.AddStratzClient()
			.ConfigureHttpClient(client =>
			{
				client.BaseAddress = new Uri("https://api.stratz.com/graphql");
				client.Timeout = TimeSpan.FromSeconds(30);
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
			});

		return services;
	}
}
