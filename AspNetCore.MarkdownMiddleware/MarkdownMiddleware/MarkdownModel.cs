using Microsoft.AspNetCore.Html;

namespace AspNetCore.MarkdownMiddleware
{
    public sealed class MarkdownModel
    {
        public string Title { get; set; }

        public string Date { get; set; }

        public HtmlString RenderedMarkdown { get; set; }

        public string PhysicalPath { get; set; }
    }
}
