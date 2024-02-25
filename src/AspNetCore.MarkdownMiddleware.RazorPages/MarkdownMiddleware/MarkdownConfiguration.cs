using System.Collections.Generic;
using System;

namespace AspNetCore.MarkdownMiddleware
{
    public sealed class MarkdownConfiguration
    {
        /// <summary>
        /// List of relative virtual folders where any (extensionless) URL is matched to an .md file on disk
        /// </summary>
        public List<string> MarkdownFolders { get; } = new List<string>();

        public string BasePath { get; set; } = "";

        /// <summary>
        /// Adds a folder where to look for Markdown files
        /// </summary>
        /// <param name="path">The path to work on.</param>
        public void AddMarkdownFolder(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path can not be null!", nameof(path));

            if (!path.StartsWith("/"))
                path = "/" + path;
            if (!path.EndsWith("/"))
                path += "/";

            MarkdownFolders.Add(path);
        }
    }
}
