using Microsoft.AspNetCore.Rewrite;

namespace AspNetCore.MarkdownMiddleware.Blazor.MarkdownMiddleware;

internal class MarkdownRedirectRule : IRule
{
    private readonly MarkdownConfiguration _configuration;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public MarkdownRedirectRule(MarkdownConfiguration markdownConfiguration, IWebHostEnvironment hostingEnvironment)
    {
        _configuration = markdownConfiguration;
        _hostingEnvironment = hostingEnvironment;
    }

    public void ApplyRule(RewriteContext context)
    {
        var request = context.HttpContext.Request;

        var path = request.Path.Value;
        if (string.IsNullOrEmpty(path) || path == "/")
            return;

        // only process *.md files if extension is available, ignore all other file extensions
        if (Path.HasExtension(path) && !path.EndsWith(".md"))
            return;

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
            context.HttpContext.Items["MarkdownModel"] = model;

            // rewrite path to a generic markdown rendering controller
            request.Path = "/Markdown/MarkdownRenderingPage";
            context.Result = RuleResult.SkipRemainingRules;

            return;
        }
    }
}
