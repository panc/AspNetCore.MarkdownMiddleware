# Asp.NET Core Markdown Middleware

A sample of a Asp.NET Core middleware for serving markdown files as rendered HTML.

This sample
- uses Razor Pages for rendering regular sites
- serves *.md files as rendered HTML
- maps extensionless URLs to the markdown files
- uses [MarkDig Markdown Parser](https://github.com/lunet-io/markdig) for parsing and converting markdown files
- inspired by https://github.com/RickStrahl/Westwind.AspNetCore.Markdown
