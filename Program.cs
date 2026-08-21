using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Data;
using PindahWebsite3.Areas.Identity.Data;
using PindahWebsite3.Jobs;
using PindahWebsite3.Routing;
using PindahWebsite3.Services.Zimsec;
using Quartz;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("PindahWebsite3ContextConnection") ?? throw new InvalidOperationException("Connection string 'PindahWebsite3ContextConnection' not found.");;

builder.Services.AddDbContext<PindahWebsite3Context>(options => options.UseSqlite(connectionString));

var zimsecConnection = builder.Configuration.GetConnectionString("ZimsecContextConnection") ?? "Data Source=zimsec.db";
builder.Services.AddDbContext<ZimsecContext>(options => options.UseSqlite(zimsecConnection));

builder.Services.AddScoped<IZimsecCatalogService, ZimsecCatalogService>();
builder.Services.AddScoped<IZimsecSearchService, ZimsecSearchService>();
builder.Services.AddScoped<IZimsecLibraryIndexer, ZimsecLibraryIndexer>();
builder.Services.AddScoped<ZimsecAuthService>();

builder.Services.AddDefaultIdentity<PindahWebsite3User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PindahWebsite3Context>();

builder.Services.AddAuthentication()
    .AddCookie(ZimsecAuthDefaults.Scheme, options =>
    {
        options.Cookie.Name = ZimsecAuthDefaults.CookieName;
        options.LoginPath = "/Zimsec";
        options.AccessDeniedPath = "/Zimsec";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
    });

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Configure max request limits for large bulk uploads
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 209715200; // 200 MB
});
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 209715200; // 200 MB
});

// SEO: URL routing optimization
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
    options.AppendTrailingSlash = false;
});

// SEO: Response compression for Core Web Vitals (skipped in Development so browser refresh / hot reload can inject HTML)
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
    });
}

builder.Services.AddHttpClient<PindahWebsite3.Services.OllamaChatService>();
builder.Services.AddScoped<PindahWebsite3.Services.SalesAgentService>();


// Quartz: Scheduled news generation (runs daily at 02:00 UTC)
builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("NewsGenerationJob");
    q.AddJob<NewsGenerationJob>(jobKey);
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("NewsGenerationJob-trigger")
        .WithCronSchedule("0 0 */4 * * ?")
        // Runs every 4 hours: 00:00, 04:00, 08:00, 12:00, 16:00, 20:00 UTC
    );
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
builder.Services.AddHostedService<ZimsecLibrarySyncHostedService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PindahWebsite3Context>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<PindahWebsite3User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    PindahWebsite3.Data.DbSeeder.SeedAsync(context, userManager, roleManager, configuration).Wait();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
if (!app.Environment.IsDevelopment())
{
    app.UseResponseCompression();
}
app.UseResponseCaching();
app.UseRouting();

app.Use(async (context, next) =>
{
    // Block direct PDF folder access only (o-level/a-level paths), not /zimsec controller routes.
    if (context.Request.Path.StartsWithSegments("/zimsec", out var remainder) &&
        (remainder.StartsWithSegments("/o-level") || remainder.StartsWithSegments("/a-level")))
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        return;
    }
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "sop",
    pattern: "sop",
    defaults: new { controller = "Sop", action = "Index" });

app.MapControllerRoute(
    name: "seo-landing",
    pattern: "{slug}",
    defaults: new { controller = "Seo", action = "Landing" },
    constraints: new { slug = new SeoSlugRouteConstraint() });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


app.Run();
