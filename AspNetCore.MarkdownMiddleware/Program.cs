using AspNetCore.MarkdownMiddleware;

[assembly: CLSCompliant(false)]

var builder = WebApplication.CreateBuilder();

// =============================
// Add services to the container
// =============================

// setup markdown middleware
builder.Services.AddMarkdown(config =>
{
    config.BasePath = "md";

    config.AddMarkdownFolder("/Samples");
    config.AddMarkdownFolder("/Others");
});

builder.Services.AddRazorPages();


// =====================
// Configure Application
// =====================

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// must be configured before app.UseRouting()!
app.UseMarkdown();

app.UseRouting();
app.UseStaticFiles();

app.MapRazorPages();


// ===============
// Run Application
// ===============

app.Run();