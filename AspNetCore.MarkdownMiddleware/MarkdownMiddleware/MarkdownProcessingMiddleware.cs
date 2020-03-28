using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace AspNetCore.MarkdownMiddleware
{
    public class MarkdownProcessingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MarkdownConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public MarkdownProcessingMiddleware(RequestDelegate next, MarkdownConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _next = next;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public Task InvokeAsync(HttpContext context)
        {
            var path = context?.Request.Path.Value;
            if (string.IsNullOrEmpty(path) || path == "/")
                return _next(context);

            // only process *.md files if extension is available, ignore all other file extensions
            if (Path.HasExtension(path) && !path.EndsWith(".md"))
                return _next(context);

            var slash = Path.DirectorySeparatorChar.ToString();

            var relativePath = path
                .Replace("/", slash)
                .Replace("\\", slash)
                .Replace(slash + slash, slash)
                .Substring(1);

            var physicalPath = Path.Combine(_hostingEnvironment.WebRootPath, _configuration.BasePath, relativePath);

            foreach (var folder in _configuration.MarkdownFolders)
            {
                if (!path.StartsWith(folder))
                    continue;

                // automatically process extensionless urls
                if (!physicalPath.EndsWith(".md"))
                    physicalPath += ".md";

                if (!File.Exists(physicalPath))
                    continue;

                var model = new MarkdownModel 
                {
                    PhysicalPath = physicalPath
                };

                // store the model in the HTTP context so that the controller gets access to it
                context.Items["MarkdownModel"] = model;

                // rewrite path to a generic markdown rendering controller
                context.Request.Path = "/Markdown/MarkdownRenderingPage";

                break;
            }

            return _next(context);
        }
    }
}