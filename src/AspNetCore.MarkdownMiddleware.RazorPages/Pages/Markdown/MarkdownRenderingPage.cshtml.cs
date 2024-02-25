using Markdig;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace AspNetCore.MarkdownMiddleware.Pages.Markdown
{
    public class MarkdownRenderingPageModel : PageModel
    {
        public MarkdownRenderingPageModel()
        {
        }

        public string Title { get; private set; }

        public string Date { get; private set; }

        public HtmlString RenderedMarkdown { get; private set; }

        public async Task OnGetAsync()
        {
            // retrive the model saved in the markdown processing middleware
            if (!(HttpContext.Items["MarkdownModel"] is MarkdownModel model))
                throw new InvalidOperationException("No 'MarkdownModel' found in HttpContext.Items.");

            if (!System.IO.File.Exists(model.PhysicalPath))
                return;

            string markdownContent = await System.IO.File.ReadAllTextAsync(model.PhysicalPath).ConfigureAwait(false);

            if (string.IsNullOrEmpty(markdownContent))
                return;

            markdownContent = markdownContent.Replace("\r\n", "\n");

            // interpret toml frontmatter
            string toml = null;
            if (markdownContent.StartsWith("+++"))
                toml = _FindString(markdownContent, "+++", "+++");

            if (!string.IsNullOrEmpty(toml))
            {
                var dateString = _FindString(toml, "date = \"", "\"\n");
                if (DateTime.TryParse(dateString, out var date))
                    dateString = date.ToLongDateString() + " " + date.ToLongTimeString();

                Title = _FindString(toml, "title = \"", "\"\n");
                Date = dateString;

                // remove toml frontmatter from markdown content
                markdownContent = markdownContent.Substring(toml.Length + 8);
            }

            var _pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseEmojiAndSmiley()
                .Build();

            RenderedMarkdown = new HtmlString(Markdig.Markdown.ToHtml(markdownContent, _pipeline));
        }

        private static string _FindString(string source, string start, string end)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;

            var startIndex = source.IndexOf(start, 0, source.Length);

            if (startIndex == -1)
                return string.Empty;

            var endIndex = source.IndexOf(end, startIndex + start.Length);

            if (startIndex > -1 && endIndex > 1)
                return source.Substring(startIndex + start.Length, endIndex - startIndex - start.Length);

            return string.Empty;
        }
    }
}
