# Asp.NET Core markdown rendering for Razor Pages and Blazor

A sample of a Asp.NET Core solution for serving markdown files as rendered HTML.

This sample
- serves *.md files as rendered HTML
- maps extensionless URLs to the markdown files
- uses [MarkDig Markdown Parser](https://github.com/lunet-io/markdig) for parsing and converting markdown files
- inspired by https://github.com/RickStrahl/Westwind.AspNetCore.Markdown

For rendering two technologies are used:
- Razor Pages (cshtml)
- Blazor with interactive server-side rendering (interactive SSR)