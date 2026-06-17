using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Data;
using PindahWebsite3.Areas.Identity.Data;
using PindahWebsite3.Jobs;
using Quartz;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("PindahWebsite3ContextConnection") ?? throw new InvalidOperationException("Connection string 'PindahWebsite3ContextConnection' not found.");;

builder.Services.AddDbContext<PindahWebsite3Context>(options => options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<PindahWebsite3User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PindahWebsite3Context>();

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

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PindahWebsite3Context>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<PindahWebsite3User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    PindahWebsite3.Data.DbSeeder.SeedAsync(context, userManager, roleManager, configuration).Wait();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

// SEO: Status code pages for proper HTTP responses
app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


app.Run();
