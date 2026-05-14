using Microsoft.EntityFrameworkCore;
using ScholarBridge.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ScholarBridgeContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("baglan")));

// We are going to use cookies for authentication.
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.Cookie.Name = "ScholarBridgeAuth"; //  the cookie name that will be stored in the browser
        options.LoginPath = "/Account/Login";      // if a user tries to access a page that requires authorization without being logged in, they will be redirected here
    });                                            
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  // first verify that the user is logged in (Authentication)                       
app.UseAuthorization();  // then check if the user has the required authorization (Authorization) 
                           

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
