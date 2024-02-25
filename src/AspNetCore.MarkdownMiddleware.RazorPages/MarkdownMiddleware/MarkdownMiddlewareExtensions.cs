using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspNetCore.MarkdownMiddleware
{
    public static class MarkdownMiddlewareExtensions
    {
        /// <summary>
        /// Configure the Markdown processing.
        /// </summary>
        public static IServiceCollection AddMarkdown(this IServiceCollection services, Action<MarkdownConfiguration> configAction = null)
        {
            var config = new MarkdownConfiguration();
            configAction?.Invoke(config);

            services.AddSingleton(config);

            return services;
        }

        /// <summary>
        /// Add middleware for Markdown processing. Must be configured before app.UseRouting()!
        /// </summary>
        public static IApplicationBuilder UseMarkdown(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MarkdownProcessingMiddleware>();
        }
    }
}
