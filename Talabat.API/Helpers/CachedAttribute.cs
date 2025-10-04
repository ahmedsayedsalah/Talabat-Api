using Talabat.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace Talabat.API.Helpers
{
	public class CachedAttribute : Attribute, IAsyncActionFilter
	{
		public CachedAttribute(int timeToLiveInSeconds)
		{
			TimeToLiveInSeconds = timeToLiveInSeconds;
		}

		public int TimeToLiveInSeconds { get; }

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			// if cachedResponse is found return response else execute endpoint then cache response

			var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
			var key = GenerateCacheKey(context.HttpContext.Request);
			var response = await cacheService.GetCacheResponseAsync(key);

			if (!string.IsNullOrEmpty(response))
			{
				context.Result = new ContentResult()
				{
					Content = response,
					ContentType = "application/json",
					StatusCode = 200
				};
				return;
			}

			var executedEndpointContext = await next();

			if (executedEndpointContext.Result is OkObjectResult result)
			{
				await cacheService.CacheResponseAsync(key, result, TimeSpan.FromSeconds(TimeToLiveInSeconds));
			}
		}

		private string GenerateCacheKey(HttpRequest request)
		{
			var key = new StringBuilder();

			key.Append(request.Path); // /api/Products
			foreach (var (k, v) in request.Query.OrderBy(x => x.Key))
			{
				key.Append($"|{k}-{v}");
			}
			return key.ToString();
		}
	}
}