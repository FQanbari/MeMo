using Data.Repositories;
using Memo.App.Data;
using Memo.App.Data.IRepository;
using Memo.App.Data.Repository;
using Memo.App.WebFramework.Middleware;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Add ConnectionString
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddMvc();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository,UserRepository>();

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.UseEndpoints(options =>
{
    options.MapControllers();
});
app.Run();
