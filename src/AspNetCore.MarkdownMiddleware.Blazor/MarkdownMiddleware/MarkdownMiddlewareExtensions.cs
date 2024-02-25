using AspNetCore.MarkdownMiddleware.Blazor.MarkdownMiddleware;
using Microsoft.AspNetCore.Rewrite;

namespace AspNetCore.MarkdownMiddleware
{
    public static class MarkdownMiddlewareExtensions
    {
        /// <summary>
        /// Configure the Markdown processing.
        /// </summary>
        public static IServiceCollection AddMarkdown(this IServiceCollection services, Action<MarkdownConfiguration> configAction)
        {
            var config = new MarkdownConfiguration();
            configAction?.Invoke(config);

            services.AddSingleton(config);
            services.AddSingleton<MarkdownRedirectRule>();

            return services;
        }

        /// <summary>
        /// Add middleware for Markdown processing. Must be configured before app.UseRouting()!
        /// </summary>
        public static IApplicationBuilder UseMarkdown(this IApplicationBuilder builder)
        {
            var rule = builder.ApplicationServices.GetService<MarkdownRedirectRule>();
            var options = new RewriteOptions()
                .Add(rule);

            return builder.UseRewriter(options);
        }
    }
}
