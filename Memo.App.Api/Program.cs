using Data.Repositories;
using ElmahCore.Mvc;
using ElmahCore.Sql;
using Memo.App.Common;
using Memo.App.Data;
using Memo.App.Data.IRepository;
using Memo.App.Data.Repository;
using Memo.App.Services.Idenitity;
using Memo.App.WebFramework.Configuration;
using Memo.App.WebFramework.Middleware;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using static Memo.App.Api.Helper.LoggingHelper;
 
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddSentry();

    var _siteSettings = builder.Configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
    builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection(nameof(SiteSettings)));
    //Add ConnectionString
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
    });

    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddMvc(options =>
    {
        options.Filters.Add(new AuthorizeFilter());
    });
    builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IJwtService, JwtService>();
    builder.Services.AddElmah<SqlErrorLog>(options =>
    {
        options.Path = _siteSettings.ElmahPath;
        options.ConnectionString = builder.Configuration.GetConnectionString("Elmah");
        //options.OnPermissionCheck = httpContext => httpContext.User.IsInRole("guest");
    });
    //builder.Services.AddControllers(options =>
    //{
    //    options.Filters.Add(new AuthorizeFilter());
    //});
    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    //other classes that need the logger 
    builder.Services.AddTransient<GenericHelper>();
    builder.Services.AddJwtAuthentication(_siteSettings.JwtSettings);
    var app = builder.Build();

    app.UseCustomExceptionHandler();
    // Configure the HTTP request pipeline.
    //if (!app.Environment.IsDevelopment())
    //{
    //    app.UseExceptionHandler(c => c.Run(async context =>
    //    {
    //        var exception = context.Features
    //            .Get<IExceptionHandlerPathFeature>()
    //            .Error;
    //        var response = new { error = exception.Message };
    //        await context.Response.WriteAsJsonAsync(response);
    //    }));
    //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //    app.UseHsts();
    //}
   
    app.UseElmah();
    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    
    app.MapRazorPages(); 
    app.UseSentryTracing();
   
    app.UseEndpoints(options =>
    {
        options.MapControllers();
    });
    app.MapControllerRoute(
       name: "default",
       pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();

}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}